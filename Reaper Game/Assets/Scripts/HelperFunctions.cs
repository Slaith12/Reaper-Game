using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class HelperFunctions
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

    public static T AddComponent<T>(this GameObject gameObject, T original) where T : Component
    {
        Type type = original.GetType(); //type may be different from T in the case of child classes (i.e. Collider)
        T copy = (T)gameObject.AddComponent(type);

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                bool obsolete = false;
                IEnumerable attrData = pinfo.CustomAttributes;
                foreach (CustomAttributeData data in attrData)
                {
                    if (data.AttributeType == typeof(System.ObsoleteAttribute))
                    {
                        obsolete = true;
                        break;
                    }
                }
                if (obsolete)
                {
                    continue;
                }
                try
                {
                    pinfo.SetValue(copy, pinfo.GetValue(original, null), null);
                }
                catch { } // In case of NotImplementedException being thrown.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(copy, finfo.GetValue(original));
        }
        return copy;
    }

    public static void SetTransform(this Transform transform, Vector3 localPosition, float rotation, Vector2 localScale)
    {
        transform.localPosition = localPosition;
        transform.eulerAngles = new Vector3(0, 0, rotation);
        transform.localScale = localScale;
    }
}
