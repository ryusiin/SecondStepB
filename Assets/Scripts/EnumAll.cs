using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumAll
{
    public enum eCharacter
    {
        SAKIRI = 1000,
        AHURA = 1001,
        HARU = 1002,
        AMY = 1010,
        RAJESH = 1011,
        ARSENE = 1012
    }

    public enum eScene
    {
        INTRO = 1,
        LOBBY = 2,
        IN_HEIST_LOBBY = 3,
        IN_HEIST = 4,
        RESULT = 5
    }

    public enum eSkill
    {
        BLUE_WATER = 1000,
        ONLY_LIGHT = 1001,
        PLATONIC_LOVE = 1002
    }

    public enum eSkillType
    {
        SELF = 100,
        DIRECTION = 101,
        RANGE = 102
    }

    public enum eStatus
    {
        HP = 1
    }

    public enum eEffect
    {
        HP_RECOVERY = 10,
        BEAM = 11,
        CURSE = 12,
        HIT = 13,
    }

    public enum eTeam
    {
        YELLOW,
        RED,
        BLUE
    }

    public enum eAnimation
    {
        IDLE,
        WALK,
        ATTACK
    }
}
