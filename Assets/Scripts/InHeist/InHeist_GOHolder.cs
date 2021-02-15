using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_GOHolder : MonoBehaviour
{
    [Header("for Characters")]
    public GameObject TRANSFORM_poolCharacters;
    public GameObject TRANSFORM_fieldCharacters;
    public GameObject TRANSFORM_poolBeam;
    public GameObject TRANSFORM_poolMissile;
    [Header("for Teams")]
    public GameObject TRANSFORM_showcaseRed;
    public GameObject TRANSFORM_showcaseBlue;

    // :: Initialise
    public void Init()
    {
        Dictator.Check_Null(this.ToString(),
            this.TRANSFORM_poolCharacters,
            this.TRANSFORM_fieldCharacters,
            this.TRANSFORM_poolBeam,
            this.TRANSFORM_poolMissile);

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }
}
