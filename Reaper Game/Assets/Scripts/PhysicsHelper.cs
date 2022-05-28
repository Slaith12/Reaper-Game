using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsHelper
{
    public static float ToAngle(this Vector2 direction)
    {
        float angle = Mathf.Atan(-direction.x / direction.y)*180/Mathf.PI;
        if (angle < 0)
            angle += 180;
        if (direction.x > 0)
            angle += 180;
        if (direction.y < 0 && angle < 90)
            angle += 180;
        return angle;
    }

    public static Vector2 ToDirection(this float angle)
    {
        float radAngle = angle * Mathf.PI / 180;
        return new Vector2(Mathf.Sin(radAngle), -Mathf.Cos(radAngle));
    }
}
