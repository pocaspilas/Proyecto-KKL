using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject coinPrefab;
    public AnimationCurve scaleAnimation;

    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (!playerLayer.Matches(other.gameObject.layer)) return;
        Debug.Log("END");
        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        PlayerController.instance.ShowPanel("¡Es tiempo de plantar arboles!");
        yield return new WaitForSeconds(3);
        PlayerController.instance.HidePanel();

        yield return new WaitForSeconds(1);
        PlayerController.instance.ShowPushke();
        
        for (int i = 0; i < PlayerController.instance.itemsCollected; i++)
        {
            var obj = Instantiate(coinPrefab);
            StartCoroutine(PlantAnimation(obj));
            yield return new WaitForSeconds(.5f);
        }

        PlayerController.instance.HidePushke();
    }

    IEnumerator PlantAnimation(GameObject obj)
    {
        float time = 0;
        Vector3 initialScale = new Vector3(1, 1, 1);
        obj.transform.Rotate(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));
        while (time < 1)
        {
            obj.transform.position = BezierUtility.SimpleBezier(PlayerController.PushkePosition, target.transform.position, time, 5f);
            obj.transform.localScale = initialScale * scaleAnimation.Evaluate(time);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }
}
