using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result_UIHolder : MonoBehaviour
{
    // : Assign
    public GameObject Button_ok;
    public GameObject IMAGE_dim;
    public GameObject TEXT_result;

    // : Initialise
    public void Init()
    {
        Dictator.Check_Null(this.ToString(),
            this.Button_ok);

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }
}
