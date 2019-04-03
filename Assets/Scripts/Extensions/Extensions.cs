using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Extensions {

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
}
