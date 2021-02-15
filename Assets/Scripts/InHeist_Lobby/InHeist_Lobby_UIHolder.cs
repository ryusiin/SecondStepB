using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Lobby_UIHolder : MonoBehaviour
{
    // : Assign
    public GameObject Button_heist;
    public GameObject Image_team;
    public GameObject Text_nickname;
    public GameObject Image_flagRed;
    public GameObject Image_flagBlue;
    public GameObject IMAGE_dim;

    // : Initialise
    public void Init()
    {
        Dictator.Check_Null(this.ToString(),
            this.Button_heist);

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }
}
