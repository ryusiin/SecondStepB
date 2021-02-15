using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Leader_Tile : MonoBehaviour
{
    [Header("Status")]
    public bool isEmpty = true;
    public EnumAll.eTeam eTeam;

    [Header("Check")]
    [SerializeField]
    private InHeist_Leader_Character stayCharacter;
    public Vector3 Coordinate { get; private set; }

    // : Assign
    public GameObject IMAGE_hex;

    // :: Initialise
    public void Init(Vector3 coordinate)
    {
        // :: Init
        this.ReleaseCharacter();

        // :: Set
        this.Coordinate = coordinate;
        this.SetColor_HexImage(Dictator.eTeam);

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // :: Get
    public bool GetStatus_isEmpty()
    {
        return this.isEmpty;
    }
    public InHeist_Leader_Character GetCharacter()
    {
        return this.stayCharacter;
    }

    // :: Set
    public void SetCharacter(InHeist_Leader_Character character)
    {
        this.isEmpty = false;
        this.stayCharacter = character;
    }
    public void SetColor_HexImage(EnumAll.eTeam eTeam)
    {
        this.IMAGE_hex.GetComponent<SpriteRenderer>().color = Dictator.GetColor(eTeam);
    }

    // :: Release Character
    public void ReleaseCharacter()
    {
        this.isEmpty = true;
        this.stayCharacter = null;
    }

    // : Show
    public void Show_HexImage(bool check)
    {
        if(this.IMAGE_hex.activeSelf != check)
            this.IMAGE_hex.SetActive(check);
    }
}
