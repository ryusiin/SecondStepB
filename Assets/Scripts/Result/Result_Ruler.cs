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

        // :: Function
        this.DIMManager = new Manager_Dim(this.UIChief.GetImage_Dim());

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
}
