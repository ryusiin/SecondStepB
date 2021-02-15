using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Coordinate
{
    public static int GetDistance(Vector3 basePosition, Vector3 targetPosition)
    {
        // :: Set
        int x = (int)basePosition.x - (int)targetPosition.x;
        int y = (int)basePosition.y - (int)targetPosition.y;
        int z = (int)basePosition.z - (int)targetPosition.z;
        int distance = (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z)) / 2;

        // :: Return
        return distance;
    }
}
