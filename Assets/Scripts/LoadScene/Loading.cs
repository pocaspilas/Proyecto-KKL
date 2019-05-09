using Mati36.SceneManagement;
using Mati36.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public SceneManagementHelper helper;

    IEnumerator Start()
    {
        FadeUtility.FadeFromBlack(1);
        yield return new WaitForEndOfFrame();
        //SceneManagementHelper.ChangeScene_Static("MainScene");
        helper.LoadSceneAsync("MainScene");
    }
}
