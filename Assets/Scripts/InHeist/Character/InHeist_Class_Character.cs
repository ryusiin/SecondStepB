using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Class_Character
{
    public EnumAll.eCharacter CharacterType { get; private set; }
    public EnumAll.eTeam Team { get; private set; }

    // :: Constructor
    public InHeist_Class_Character(EnumAll.eCharacter characterType, EnumAll.eTeam team)
    {
        this.CharacterType = characterType;
        this.Team = team;
    }
}
