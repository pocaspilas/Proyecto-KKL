using Mati36.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public SceneManagementHelper helper;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        //SceneManagementHelper.ChangeScene_Static("MainScene");
        helper.LoadSceneAsync("MainScene");
    }
}
