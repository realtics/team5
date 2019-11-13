using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int upIndex = 3;
    public static int downIndex = 4;
    public static int frontIndex = 1;
    public static int downFrontIndex = 0;
    public static int upFrontIndex = 2;

    public static float AngleInRange(float value, float min)
    {
        float result = value;
        while (result < min)
            result += Mathf.PI * 2;
        while (result > min + Mathf.PI * 2)
            result -= Mathf.PI * 2;
        return result;
    }
}
