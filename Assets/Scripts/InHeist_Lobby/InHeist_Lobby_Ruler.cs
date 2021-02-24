using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Lobby_Ruler : MonoBehaviour
{
    // : Development
    public bool DoTest = false;

    // : 0 Awake
    private void Awake()
    {
        if (this.DoTest == false)
            Dictator.CheckDictator();
        else
            this.Init();
    }

    // : Chief
    private InHeist_Lobby_UIChief UIChief;
    private InHeist_Lobby_GOChief GOChief;
    private InHeist_Lobby_MULTIChief MULTIChief;

    // : Function
    private Manager_Photon PHOTONManager;
    private Manager_Dim DIMManager;

    // : Status
    const float DIM_DURATION = 1f;
    const float FADE_OUT = 0f;
    const float FADE_IN = 1f;

    // : Please
    public System.Action<EnumAll.eScene> Please_MoveScene;
    // : Initialise
    public void Init()
    {
        // :: Chief
        this.UIChief = GameObject.FindObjectOfType<InHeist_Lobby_UIChief>();
        this.UIChief.Init();
        this.GOChief = GameObject.FindObjectOfType<InHeist_Lobby_GOChief>();
        this.GOChief.Init();
        this.MULTIChief = this.GetComponent<InHeist_Lobby_MULTIChief>();
        this.MULTIChief.Init();


        // :: Function
        this.PHOTONManager = GameObject.FindObjectOfType<Manager_Photon>();
        this.PHOTONManager.Init();
        this.DIMManager = new Manager_Dim(this.UIChief.GetImage_Dim());

        // :: Scenario
        this.Scenario_ShowRandomCharacter();
        this.Scenario_JoinRoom();

        // :: Button Scenario
        this.AddButtonScenario_Heist();

        // :: Complete
        Dictator.Debug_Init(this.ToString());

        // :: Fade
        this.DIMManager.Show(true);
        this.DIMManager.Fade(FADE_OUT, DIM_DURATION, () => { this.DIMManager.Show(false); });
    }

    // : Scenario
    private void Scenario_ShowRandomCharacter()
    {
        this.GOChief.ShowCharacter_All(false);

        // :: Amy(1010) to Arsene(1012)
        int startCharacter = (int)EnumAll.eCharacter.AMY;
        int endCharacter = (int)EnumAll.eCharacter.ARSENE;
        EnumAll.eCharacter charcter = (EnumAll.eCharacter)Random.Range(startCharacter, endCharacter);
        this.GOChief.ShowCharacter(charcter);
    }
    private void Scenario_JoinRoom()
    {
        this.PHOTONManager.Callback_Joined = () =>
        {
            this.UIChief.ChangeText_Nickname(Dictator.Nickname);
            this.Scenario_Wait();
        };
        this.PHOTONManager.Try_JoinRoom();
    }
    private Coroutine waitCoroutine = null;
    private void Scenario_Wait()
    {
        this.waitCoroutine = this.StartCoroutine(this.Scenario_WaitImplement());
    }
    private IEnumerator Scenario_WaitImplement()
    {
        while(true)
        {
            // :: Team Color
            Dictator.eTeam = this.PHOTONManager.Is_MasterClient() ? EnumAll.eTeam.RED : EnumAll.eTeam.BLUE;
            this.UIChief.ChangeColor_Team(Dictator.eTeam);

            // :: Player Count
            int playerCount = this.PHOTONManager.Get_PlayerCount();
            if(playerCount == 1)
            {
                this.UIChief.Show_Flag(EnumAll.eTeam.RED, true);
                this.UIChief.Show_Flag(EnumAll.eTeam.BLUE, false);
                this.UIChief.CanButton_Heist(false);
                
                // :::: Single TEST
                //this.UIChief.CanButton_Heist(true);
            } else if(playerCount >= 2)
            {
                this.UIChief.Show_Flag(EnumAll.eTeam.RED, true);
                this.UIChief.Show_Flag(EnumAll.eTeam.BLUE, true);
                this.UIChief.CanButton_Heist(true);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // : Button Scenario
    private void AddButtonScenario_Heist()
    {
        this.UIChief.AddButtonListener_Heist(() =>
        {
            this.UIChief.CanButton_Heist(false);
            this.StopCoroutine(this.waitCoroutine);

            // :: Change Scene
            this.MULTIChief.MoveScene_InHeist();
            //this.PHOTONManager.Load_Level(EnumAll.eScene.IN_HEIST);
            
        });
    }
}
