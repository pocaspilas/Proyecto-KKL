using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Mati36.Utility
{
    public class Fade : MonoBehaviour
    {
        public Image fadeImg;
        public Color currentFadeColor = Color.black;
        public Canvas canvas;

        private void CheckCamera()
        {
            if (canvas.worldCamera == null)
                canvas.worldCamera = Camera.main;
        }

        public void FadeTo(Color color, float duration, Action callback)
        {
            CheckCamera();
            StartCoroutine(FadeRoutine(color, duration, true, callback));
        }

        public void FadeFrom(Color color, float duration, Action callback)
        {
            CheckCamera();
            StartCoroutine(FadeRoutine(color, duration, false, callback));
        }

        IEnumerator FadeRoutine(Color color, float duration, bool toBlack, Action callback)
        {
            float t = 0;
            while (t < 1)
            {
                fadeImg.color = toBlack ? Color.Lerp(Color.clear, currentFadeColor, t) : Color.Lerp(currentFadeColor, Color.clear, t);
                t += Time.deltaTime / duration;
                yield return null;
            }
            fadeImg.color = toBlack ? currentFadeColor : Color.clear;

            callback?.Invoke();
        }
    }


    static public class FadeUtility
    {
        const string PREFAB_NAME = "Mati36_FadeUtilityPrefab";

        static private Fade _fadeObj;
        static public Fade FadeObj
        {
            get
            {
                if (_fadeObj == null)
                {
                    var obj = GameObject.Find(PREFAB_NAME);
                    Fade existingFade;
                    if (obj == null)
                    {
                        existingFade = GameObject.Instantiate(Resources.Load<Fade>(PREFAB_NAME), null);
                        existingFade.name = PREFAB_NAME;
                    }
                    else
                        existingFade = obj.GetComponent<Fade>();

                    _fadeObj = existingFade;
                }
                return _fadeObj;
            }
        }


        static public void FadeToColor(Color color, float duration, Action callback = null)
        {
            FadeObj.FadeTo(color, duration, callback);
        }

        static public void FadeFromColor(Color color, float duration, Action callback = null)
        {
            FadeObj.FadeFrom(color, duration, callback);
        }

        static public void FadeToBlack(float duration, Action callback = null)
        {
            FadeObj.FadeTo(Color.black, duration, callback);
        }

        static public void FadeFromBlack(float duration, Action callback = null)
        {
            FadeObj.FadeFrom(Color.black, duration, callback);
        }
    }
}