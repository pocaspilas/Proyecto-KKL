using Mati36.SceneManagement;
using Mati36.Sound;
using Mati36.Utility;
using Mati36.VR;
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
    public SoundAsset endMusic;

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
        VRGaze.currentVRCamera.DisableReticle();
        int items = PlayerController.instance.itemsCollected;

        var music = SoundManager.PlaySound(endMusic);
        music.FadeIn(2f);
            //PlayerController.instance.currentMusic2.CrossfadeTo(endMusic, 2f);

        music.e_OnEndSound += ReturnToMenu;

        yield return new WaitForSeconds(2f);
        if (items > 0)
        {
            PlayerController.instance.ShowPanel("¡Es tiempo de <b>plantar arboles</b>!");
            yield return new WaitForSeconds(3);
            PlayerController.instance.HidePanel();

            yield return new WaitForSeconds(1);
            PlayerController.instance.ShowPushke();
            yield return new WaitForSeconds(1f);
            PlayerController.instance.SetPushkeSpeed(1f / (timeBetweenCoins / 0.5f));


            for (int i = 0; i < items; i++)
            {
                if (coinSound)
                {
                    float pitch = (((float)i) / items) / 2f + 0.5f;
                    SoundManager.PlaySoundAt(coinSound, PlayerController.instance.pushke.transform.position, pitch);
                }
                var obj = Instantiate(coinPrefab);
                StartCoroutine(PlantAnimation(obj));
                if (i < items - 1)
                    yield return new WaitForSeconds(timeBetweenCoins);
            }
            PlayerController.instance.SetPushkeSpeed(1f);
            PlayerController.instance.HidePushke();
        }

        yield return new WaitForSeconds(3f);
        if (items == 0)
            PlayerController.instance.ShowPanel("¡<b>Gracias</b> por participar de la experiencia!");
        else if (items == 1)
            PlayerController.instance.ShowPanel("¡<b>Gracias</b> por ayudarnos\n a plantar <b>un arbol</b>!");
        else
            PlayerController.instance.ShowPanel("¡<b>Gracias</b> por ayudarnos\n a plantar <b>" + (items) + " arboles</b>!");

        yield return new WaitForSeconds(3f);
        PlayerController.instance.HidePanel();
        yield return new WaitForSeconds(3f);


    }

    void ReturnToMenu(PoolableAudioSource sourceEnded)
    {
        sourceEnded.e_OnEndSound -= ReturnToMenu;
        FadeUtility.FadeToBlack(2, () => SceneManagementHelper.ChangeScene_Static("MenuScene"));
    }

    IEnumerator PlantAnimation(GameObject obj)
    {
        float time = 0;
        Vector3 initialScale = new Vector3(1, 1, 1);
        //obj.transform.Rotate(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));
        while (time < 1)
        {
            obj.transform.position = BezierUtility.SimpleBezier(PlayerController.PushkePosition, target.transform.position, time, 5f);
            obj.transform.localScale = initialScale * scaleAnimation.Evaluate(time);
            time += Time.deltaTime / coinPlantDuration;
            yield return null;
        }
        if (treeSound)
            SoundManager.PlaySoundAt(treeSound, obj.transform.position);
        plantController.Plant();
        Destroy(obj);
    }
}
