using Mati36.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mati36.SceneManagement
{
    public class SceneManagementHelper : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        static public void ChangeScene_Static(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(AsyncLoadRoutine(sceneName));
        }

        private IEnumerator AsyncLoadRoutine(string sceneName)
        {
            bool finishedLoading = false;

            var asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = false;
            while(!asyncOp.isDone)
            {
                Debug.Log("Async Progress = " + (asyncOp.progress * 100) + "%");
                if(!finishedLoading && asyncOp.progress >= 0.9f)
                {
                    finishedLoading = true;
                    FadeUtility.FadeToBlack(1, () => asyncOp.allowSceneActivation = true);
                    //asyncOp.allowSceneActivation = true;
                }
                yield return null;
            }
            
        }
    }
}