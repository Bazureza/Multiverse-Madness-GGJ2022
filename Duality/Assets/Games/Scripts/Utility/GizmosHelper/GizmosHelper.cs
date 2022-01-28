using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosHelper
{
    public static void DrawRect(Vector2 point1, Vector2 point2, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(point1, new Vector2(point1.x, point2.y));
        Gizmos.DrawLine(new Vector2(point1.x, point2.y), point2);
        Gizmos.DrawLine(point2, new Vector2(point2.x, point1.y));
        Gizmos.DrawLine(new Vector2(point2.x, point1.y), point1);
    }
}
