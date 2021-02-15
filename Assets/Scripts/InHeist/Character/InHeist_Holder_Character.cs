using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Holder_Character : MonoBehaviour
{
    // :: Characters
    [Header("Character : 1000 / SAKIRI")]
    public GameObject character_1000;
    [Header("Character : 1001 / AHURA")]
    public GameObject character_1001;
    [Header("Character : 1002 / HARU")]
    public GameObject character_1002;
    [Header("Character : 1010 / AMY")]
    public GameObject character_1010;
    [Header("Character : 1011 / RAJESH")]
    public GameObject character_1011;
    [Header("Character : 1012 / ARSENE")]
    public GameObject character_1012;
    [Header("Effect")]
    public GameObject EFFECT_position;
    public GameObject EFFECT_heal;
    public GameObject EFFECT_curse;
    public GameObject EFFECT_zoneCurse;
    public GameObject EFFECT_hit;
    [Header("Effect for Team")]
    public GameObject IMAGE_fillHP;
    [Header("Progress Bar")]
    public GameObject SLIDER_mp;
    public GameObject SLIDER_hp;

    // :: Initialise
    public void Init()
    {
        Dictator.Check_Null(this.ToString(),
            this.character_1000,
            this.character_1001,
            this.character_1002,
            this.character_1010,
            this.character_1011,
            this.character_1012,
            this.EFFECT_position,
            this.EFFECT_heal,
            this.EFFECT_curse,
            this.EFFECT_zoneCurse,
            this.EFFECT_hit,
            this.IMAGE_fillHP,
            this.SLIDER_mp,
            this.SLIDER_hp);

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }
}
