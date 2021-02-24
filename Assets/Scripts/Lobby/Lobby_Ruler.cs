using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Ruler : MonoBehaviour
{
    // : Check Dictator
    private void Awake()
    {
        Dictator.CheckDictator();
    }

    // : Test : GPGS
    public Text TEXT_ID;
    public Text TEXT_UserName;
    public Text TEXT_State;
    public Text TEXT_Score;
    private int score;
    public Image IMAGE_Profile;
    public Button BUTTON_Archivement;
    public Button BUTTON_Defence;
    public Button BUTTON_Leaderboard;
    public Button BUTTON_Score;

    // : Chief
    private Lobby_UIChief UIChief;

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
        this.UIChief = GameObject.FindObjectOfType<Lobby_UIChief>();
        this.UIChief.Init();

        // : Function
        this.PHOTONManager = GameObject.FindObjectOfType<Manager_Photon>();
        this.PHOTONManager.Init();
        this.DIMManager = new Manager_Dim(this.UIChief.GetImage_Dim());

        // :: Set
        this.Set_Nickname();

        // :: Button Scenario
        this.AddButtonScenario_Heist();

        // :: Complete
        Dictator.Debug_Init(this.ToString());

        // :: Fade
        this.DIMManager.Show(true);
        this.DIMManager.Fade(FADE_OUT, DIM_DURATION, () => { this.DIMManager.Show(false); });
    }

    // : Set
    public void Set_Nickname()
    {
        Dictator.Nickname = System.Guid.NewGuid().ToString();
    }

    // : Scenario
    private void AddButtonScenario_Heist()
    {
        this.UIChief.AddButtonListener_Heist(() =>
        {
            // :: Dis
            this.UIChief.CanButton_Heist(false);

            // :: Connect
            this.PHOTONManager.Callback_Connected = () =>
            {
                this.DIMManager.Show(true);
                this.DIMManager.Fade(FADE_IN, DIM_DURATION, () => {
                    this.Please_MoveScene(EnumAll.eScene.IN_HEIST_LOBBY);
                });
            };
            this.PHOTONManager.Try_Connect();
        });
    }
}
