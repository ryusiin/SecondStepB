using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_UIHolder : MonoBehaviour
{
    // : Assign
    public GameObject BUTTON_profile;
    public GameObject BUTTON_stage;
    public GameObject BUTTON_heist;
    public Button BUTTON_OpenStory;
    public Button BUTTON_StoryHeist;
    public GameObject IMAGE_buttonHeist;
    public GameObject IMAGE_dim;

    // : Initialise
    public void Init()
    {
        Dictator.Check_Null(this.ToString(),
            this.BUTTON_profile,
            this.BUTTON_stage,
            this.BUTTON_heist,
            this.IMAGE_dim);

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }
}
