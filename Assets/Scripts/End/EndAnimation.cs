using Mati36.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject coinPrefab;
    public AnimationCurve scaleAnimation;

    public PlantController plantController;

    public Transform target;

    public SoundAsset coinSound, treeSound;

    [Header("Animation")]
    public float timeBetweenCoins;
    public float coinPlantDuration;

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
        yield return new WaitForSeconds(1f);
        PlayerController.instance.SetPushkeSpeed(1f / (timeBetweenCoins / 0.5f));

        for (int i = 0; i < PlayerController.instance.itemsCollected; i++)
        {
            if (coinSound)
                SoundManager.PlayOneShotSound(coinSound);
            var obj = Instantiate(coinPrefab);
            StartCoroutine(PlantAnimation(obj));
            if (i < PlayerController.instance.itemsCollected - 1)
                yield return new WaitForSeconds(timeBetweenCoins);
        }
        PlayerController.instance.SetPushkeSpeed(1f);
        PlayerController.instance.HidePushke();

        yield return new WaitForSeconds(3f);
        PlayerController.instance.ShowPanel("¡Gracias por ayudarnos a plantar " + PlayerController.instance.itemsCollected + " arboles!");
        yield return new WaitForSeconds(3f);
        PlayerController.instance.HidePanel();
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
            time += Time.deltaTime / coinPlantDuration;
            yield return null;
        }
        if (treeSound)
            SoundManager.PlayOneShotSound(treeSound);
        plantController.Plant();
        Destroy(obj);
    }
}
