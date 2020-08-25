using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 RoundToNearest(this Vector3 v, float toNearest = 1, bool ignoreY = false)
    {
        v.x = Mathf.Round(v.x / toNearest) * toNearest;
        if(!ignoreY)
            v.y = Mathf.Round(v.y / toNearest) * toNearest;
        v.z = Mathf.Round(v.z / toNearest) * toNearest;

        return v;
    }
}
