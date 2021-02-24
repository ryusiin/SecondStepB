using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result_Ruler : MonoBehaviour
{
    // : 0 Awake
    private void Awake()
    {
        Dictator.CheckDictator();
    }

    // : Chief
    private Result_UIChief UIChief;
    private Result_GOChief GOChief;

    // : Function
    private Manager_Dim DIMManager;

    // : Please
    public System.Action<EnumAll.eScene> Please_MoveScene;
    // : Initialise
    public void Init()
    {
        // :: Chief
        this.UIChief = GameObject.FindObjectOfType<Result_UIChief>();
        this.UIChief.Init();
        this.GOChief = GameObject.FindObjectOfType<Result_GOChief>();
        this.GOChief.Init();

        // :: Function
        this.DIMManager = new Manager_Dim(this.UIChief.GetImage_Dim());

        // :: Scenario
        this.Scenario_SetWinLose();
        this.Scenario_ShowRandomCharacter();

        // :: Button Scenario
        this.AddButtonScenario_OK();

        // :: Complete
        Dictator.Debug_Init(this.ToString());

        // :: Fade
        this.DIMManager.Show(true);
        this.DIMManager.Fade(0f, 1f, () => { this.DIMManager.Show(false); });

    }

    // : Button Scenario
    private void AddButtonScenario_OK()
    {
        this.UIChief.AddButtonListener_OK(() =>
        {
            this.DIMManager.Show(true);
            this.DIMManager.Fade(1f, 1f, () => {
                this.Please_MoveScene(EnumAll.eScene.LOBBY);
            });
        });
    }

    // : Scenario
    private void Scenario_SetWinLose()
    {
        EnumAll.eResult eResult = Dictator.eResult;
        if (eResult == EnumAll.eResult.WIN)
            this.UIChief.SetText_Result("YOU WIN");
        else if (eResult == EnumAll.eResult.LOSE)
            this.UIChief.SetText_Result("YOU LOSE");
    }
    private void Scenario_ShowRandomCharacter()
    {
        this.GOChief.ShowCharacter_All(false);

        // :: Amy(4) to Arsene(6)
        int startCharacter = (int)EnumAll.eCharacter.AMY;
        int endCharacter = (int)EnumAll.eCharacter.ARSENE; 
        EnumAll.eCharacter charcter = (EnumAll.eCharacter)Random.Range(startCharacter, endCharacter);
        this.GOChief.ShowCharacter(charcter);
    }
}
