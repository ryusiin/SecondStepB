using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Activist_Drag : MonoBehaviour
{
    // :: Constant
    // :: Layer Number
    const int LAYER_SHOWCASE = 10;
    // :: Layer Mask
    const int LAYER_MASK_MAP = 1 << 11; // :: Map
    const int LAYER_MASK_SHOWCASE = 1 << 10; // :: Showcase
    const int LAYER_MASK_CHAMPION = 1 << 9; // :: Champion
    const int LAYER_MASK_TILE = 1 << 8; // :: Tile

    // : Activist
    private InHeist_Activist_Map MAPActivist;

    // : Init
    public void Init()
    {
        // :: Activist
        this.MAPActivist = GameObject.FindObjectOfType<InHeist_Activist_Map>();
    
        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // :: 8 Update
    private bool onDrag; // :: 지금 드래그 중인지 확인
    private Transform onChamp; // :: 지금 움직이고 있는 챔피언
    private Vector3 onChampFirstPosition; // :: 챔피언의 원래 포지션
    private void Update()
    {
        // :: 마우스 클릭
        if (Input.GetMouseButtonDown(0))
        {
            this.RayCastToChampion(); // :: 레이캐스트
        }

        // :: Drag Start
        if (this.onDrag)
        {
            this.MAPActivist.Show_AllHexImage(true, Dictator.eTeam);
            this.Move(); // :: 이동
        }

        // :: Mouse Up : Drag End
        if (this.onDrag && Input.GetMouseButtonUp(0))
        {
            this.onDrag = false; // :: 드래그 끝
            this.SetOnTile(); // :: 타일에 챔피언을 셋
            this.MAPActivist.Show_AllHexImage(false);
        }
    }

    // :: RayCast
    private void RayCastToChampion()
    {
        // :: for Use
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        int layerMask = LAYER_MASK_SHOWCASE | LAYER_MASK_CHAMPION; // :: 레이어 마스크

        // :: 카메라 레이
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            EnumAll.eTeam characterTeam = hit.transform.GetComponent<InHeist_Leader_Character>().GetTeamColor();
            Debug.LogFormat("::::::::::::::: {0} 테스트 중 이곳 수정할 것", this.ToString());
            //if (characterTeam == Dictator.eTeam)
            if(true)
            {
                this.onDrag = true; // :: 드래그 시작
                this.onChamp = hit.transform; // :: 현재 챔피언 저장
                this.onChampFirstPosition = this.onChamp.transform.position; // :: 원래 위치 저장
            }
        }
    }

    // :: Drag Move
    private void Move()
    {
        // :: for Use
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();

        // :: 카메라 레이
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // :: 맵 포지션
        Vector3 mapPosition = Vector3.zero;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, LAYER_MASK_MAP))
        {
            mapPosition = hit.point; // :: 맵의 위치
            this.onChamp.position = mapPosition; // 챔피언 이동
        }
    }

    // :: 타일에 챔피언 셋
    public System.Action<InHeist_Class_Character, InHeist_Leader_Tile> Please_SetNewChampion = null;
    public System.Action<Transform, InHeist_Leader_Tile> Please_SetChampion = null;
    private void SetOnTile()
    {
        // :: for Use
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();

        // :: Camera Ray
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // :: Map Position
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, LAYER_MASK_TILE)) // :: 해당 위치가 타일이면
        {
            // :: Easy To Use
            var tile = hit.transform.GetComponent<InHeist_Leader_Tile>();

            if (tile.GetStatus_isEmpty() == true && tile.eTeam == Dictator.eTeam)
            {
                if (this.onChamp.gameObject.layer == LAYER_SHOWCASE) // :: 혹시 지금 챔피언이 지금 쇼케이스 레이어 마스크면
                {
                    // :: Get Data
                    InHeist_Class_Character characterStatus = this.onChamp.GetComponent<InHeist_Leader_Character>().GetCharacterStatus();

                    // :: Request to Ruler
                    this.Please_SetNewChampion?.Invoke(characterStatus, tile);

                    // :: Return Showcase Champion
                    this.onChamp.localPosition = new Vector3(0, 0.25f, 0);
                }
                else
                {
                    // :: Set
                    this.Please_SetChampion.Invoke(this.onChamp, tile);
                }
            } 
            else
            {
                this.onChamp.position = this.onChampFirstPosition;
            }
        }
        else // :: 해당 위치가 타일이 아니면
        {
            // :: Return Champion
            this.onChamp.position = this.onChampFirstPosition;
        }
    }
}
