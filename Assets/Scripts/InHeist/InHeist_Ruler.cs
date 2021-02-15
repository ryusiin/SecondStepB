using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class InHeist_Ruler : MonoBehaviour
{
    // : Development
    public bool DoTest = false;
    public EnumAll.eTeam testTeam;

    // : 0 Awake
    private void Awake()
    {
        bool check = Dictator.CheckDictator();
        if (check == true)
            this.Init();
    }

    // :: Singleton
    private PVPBattle_Sing_DataController DataController; // :: Saving Data

    // :: Activist
    private InHeist_Activist_Drag DRAGActivist;
    private InHeist_Activist_Map MAPActivist;

    // :: Chief
    private InHeist_GOChief GOChief;
    private InHeist_UIChief UIChief;

    // : Function
    private Manager_Photon PHOTONManager;
    private Manager_PhotonRPC_InHeist RPCManager;
    private Manager_Dim DIMManager;

    // :: Status
    private Vector3 ADJUSTMENT_POSITION = new Vector3(0, 0.25f, 0);
    const float DIM_DURATION = 1f;
    const float FADE_OUT = 0f;
    const float FADE_IN = 1f;

    // : Please
    public System.Action<EnumAll.eScene> Please_MoveScene;
    // :: Section : Initialise
    public void Init()
    {
        // :: Singleton
        this.DataController = PVPBattle_Sing_DataController.Instance();

        // :: Activist
        this.DRAGActivist = GameObject.FindObjectOfType<InHeist_Activist_Drag>();
        this.DRAGActivist.Init();
        this.MAPActivist = GameObject.FindObjectOfType<InHeist_Activist_Map>();
        this.MAPActivist.Init();

        // :: Chief
        this.GOChief = GameObject.FindObjectOfType<InHeist_GOChief>();
        this.GOChief.Init();
        this.UIChief = GameObject.FindObjectOfType<InHeist_UIChief>();
        this.UIChief.Init();

        // :: Function
        this.PHOTONManager = GameObject.FindObjectOfType<Manager_Photon>();
        this.RPCManager = GameObject.FindObjectOfType<Manager_PhotonRPC_InHeist>();
        this.DIMManager = new Manager_Dim(this.UIChief.GetImage_Dim());

        // :: Set Button Scenario
        this.AddButtonScenario_StartBattle();

        // :: Set Answers
        this.Answer_SetNewChampion();
        this.Answer_SetChampion();

        // :: Do Scenario
        this.Scenario_Start(Dictator.eTeam);

        // :: Complete
        Dictator.Debug_Init(this.ToString());

        // :: Fade
        this.DIMManager.Show(true);
        this.DIMManager.Fade(FADE_OUT, DIM_DURATION, () => this.DIMManager.Show(false));
    }

    // : Set
    public void Set_Please()
    {
        this.RPCManager.Please_MoveScene = this.Please_MoveScene;
    }

    // : Scenario
    private bool check_GameOver = false;
    private void Scenario_Start(EnumAll.eTeam eTeam)
    {
        Debug.LogFormat("::::::::::::::: {0} 테스트 중 이곳 수정할 것", this.ToString());
        //this.GOChief.ShowShowcase(eTeam);

        // :: Show
        this.MAPActivist.Show_AllHexImage(false);
        this.MAPActivist.Show_AllHexImage(true, eTeam);
        
        // :: Set
        this.UIChief.SetTeam(eTeam, Dictator.Nickname);

        // :: Answer
        this.RPCManager.Please_DoScenarioWin = (winTeam) =>
        {
            this.Scenario_Win(winTeam);
        };
        this.RPCManager.Callback_CompleteCountDown = () => {
            this.Scenario_AutoBattle();
        };
    }
    private void Scenario_Win(EnumAll.eTeam eTeam)
    {
        if (this.check_GameOver == true)
            return;

        this.check_GameOver = true;
        this.PHOTONManager.RPC_Win(eTeam);
    }
    private void Scenario_AutoBattle()
    {
        this.PHOTONManager.RPC_Do_AutoBattle();

        // :: for USe
        List<InHeist_Leader_Character> listRedTeam = this.MAPActivist.GetTeam(EnumAll.eTeam.RED);
        List<InHeist_Leader_Character> listBlueTeam = this.MAPActivist.GetTeam(EnumAll.eTeam.BLUE);
        List<InHeist_Leader_Tile> listEmptyTiles = this.MAPActivist.GetEmptyTile();

        // :: Random Order
        int rand = Random.Range(0, 2);

        // :: Do
        if (rand == 0)
        {
            this.AutoBattle(listRedTeam);
            this.AutoBattle(listBlueTeam);
        }
        else
        {
            this.AutoBattle(listBlueTeam);
            this.AutoBattle(listRedTeam);
        }
    }

    // : Add Button Scenario
    private void AddButtonScenario_StartBattle()
    {
        this.UIChief.AddButtonListener_StartBattle(() =>
        {
            this.Scenario_AutoBattle();
        });
    }

    // : Auto
    private void AutoBattle(List<InHeist_Leader_Character> listTeam)
    {
        foreach(var itm in listTeam)
        {
            var nearEnemy = itm.AutoBattle_GetNearEnemy();
            if (nearEnemy != null)
            {
                // :: Auto Battle
                itm.AutoBattle();

                // ::Set
                itm.Please_KillThisCharacter = (guid) =>
                {
                    this.PHOTONManager.RPC_Kill_Character(guid);
                };
            }
        }
    }

    // :: Answer to request
    private void Answer_SetNewChampion()
    {
        this.DRAGActivist.Please_SetNewChampion = (characterStatus, tile) =>
        {
            string guid = System.Guid.NewGuid().ToString();

            // :: RPC
            this.PHOTONManager.RPC_Set_NewCharacter(characterStatus.CharacterType, characterStatus.Team, tile.Coordinate, guid);
        };
    }
    private void Answer_SetChampion()
    {
        this.DRAGActivist.Please_SetChampion = (characterTransform, tile) =>
        {
            string guid = characterTransform.GetComponent<InHeist_Leader_Character>().GetGUID();

            // :: RPC
            this.PHOTONManager.RPC_Set_Character(guid, tile.Coordinate);
        };
    }

}
