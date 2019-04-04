using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardConfig : MonoBehaviour
{
    public Color billboardTint;

    private void OnValidate()
    {
        Shader.SetGlobalColor("_BillboardTint", billboardTint);
    }
}
