using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extender
{
    public static Vector2 ToVec2(this Vector3 value)
    {
        return value;
    }

    public static Vector3 ToVec3(this Vector2 value)
    {
        return value;
    }

    public static void ClampVector(this ref Vector2 value, Vector2 min, Vector2 max)
    {
        Vector2 tempValue = value;
        tempValue.x = Mathf.Clamp(tempValue.x, min.x, max.x);
        tempValue.y = Mathf.Clamp(tempValue.y, min.y, max.y);

        value = tempValue;
    }

    public static bool IsNaN(this ref Vector2 value)
    {
        return float.IsNaN(value.x) || float.IsNaN(value.y);
    }
}
