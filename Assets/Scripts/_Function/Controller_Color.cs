using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Color
{
    public static Color GetColor(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
