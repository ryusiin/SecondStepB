using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Holder_Character : MonoBehaviour
{
    // :: Characters
    [Header("Character : 4 / AMY")]
    public GameObject character_4;
    [Header("Character : 5 / RAJESH")]
    public GameObject character_5;
    [Header("Character : 6 / ARSENE")]
    public GameObject character_6;
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
            this.character_4,
            this.character_5,
            this.character_6,
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
