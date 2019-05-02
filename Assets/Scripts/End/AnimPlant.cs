using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlant : MonoBehaviour
{
    public float maxSize;
    public float speed;
    public AnimationCurve growCurve;
    IEnumerator Start()
    {
        float r = Random.Range(0.4f, 1);
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.one * maxSize * t * r * growCurve.Evaluate(t); 
            t += Time.deltaTime * speed;
            yield return null;
        }
        transform.localScale = Vector3.one * maxSize * r;
        yield break;
    }
}
