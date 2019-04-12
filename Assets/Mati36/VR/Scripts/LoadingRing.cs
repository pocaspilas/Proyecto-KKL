using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mati36.UI
{
    public class LoadingRing : MonoBehaviour
    {
        public Image fillImage;
        private float fillStartTime, fillFinishTime;
        private bool isLoading;

        private Animator ani;

        public System.Action OnFillComplete = delegate { };

        private void Awake()
        {
            ani = GetComponent<Animator>();
        }

        private void Start()
        {
            isLoading = false;
            fillImage.fillAmount = 0;
        }

        private void Update()
        {
            if (isLoading)
            {
                if (Time.time < fillFinishTime)
                {
                    fillImage.fillAmount = Mathf.Lerp(0, 1, (Time.time - fillStartTime) / (fillFinishTime - fillStartTime));
                }
                else //Terminó el fill
                {
                    isLoading = false;
                    fillImage.fillAmount = 1;
                    ani.Play("RingComplete");
                    OnFillComplete();
                }
            }
        }

        public void StartFill(float timeToFill)
        {
            isLoading = true;
            fillStartTime = Time.time;
            fillFinishTime = Time.time + timeToFill;
            ani.Play("RingShow");
        }

        public void StopFill()
        {
            if (isLoading)
            {
                isLoading = false;
                fillImage.fillAmount = 0;
                ani.Play("RingHide");
            }
        }
    }
}