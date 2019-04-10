using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlant : MonoBehaviour
{
    public AnimationCurve growCurve;
    IEnumerator Start()
    {
        float r = Random.Range(0.4f, 1);
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.one * t * r; 
            t += Time.deltaTime * 0.5f;
            yield return null;
        }
        transform.localScale = Vector3.one * r;
        yield break;
    }
}
