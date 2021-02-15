using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_UIHolder : MonoBehaviour
{
    public GameObject BUTTNON_startBattle;
    public GameObject IMAGE_team;
    public GameObject IMAGE_dim;
    public GameObject TEXT_nickname;
    public GameObject TEXT_remainingTime;

    // :: Initialise
    public void Init()
    {
        Dictator.Check_Null(this.ToString(),
            this.BUTTNON_startBattle,
            this.TEXT_remainingTime);

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }
}
