using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Extensions
{
    //STRING
    static public string Colorized(this string text, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
    }

    static public string Sized(this string text, int fontSize)
    {
        return $"<size={fontSize}>{text}</size>";
    }

    //LAYERMASK
    static public bool Matches(this LayerMask layer2, LayerMask layer1)
    {
        return (((1 << layer1) & layer2) != 0);
    }

    //VECTORS
    static public Vector3 SetX(this Vector3 vector, float value)
    {
        vector.x = value;
        return vector;
    }

    static public Vector3 SetY(this Vector3 vector, float value)
    {
        vector.y = value;
        return vector;
    }

    static public Vector3 SetZ(this Vector3 vector, float value)
    {
        vector.z = value;
        return vector;
    }

    static public Vector2 IgnoreY(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    static public Vector2 XY(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    static public Vector2 XZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
    static public Vector2 YZ(this Vector3 vector)
    {
        return new Vector2(vector.y, vector.z);
    }
}
