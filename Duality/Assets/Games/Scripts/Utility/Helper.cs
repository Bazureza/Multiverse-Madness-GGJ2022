using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper 
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static float Vector2ToDegree(Vector2 direction)
    {
        direction.Normalize();
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public static int RandomSign()
    {
        return Random.value < .5 ? -1 : 1;
    }
}
