using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mati36.VR
{
    [RequireComponent(typeof(VRInteractableItem))]
    public class VRButton : MonoBehaviour
    {
        SpriteRenderer spriteRend;
        Image image;
        BoxCollider boxCol;

        [Header("Color")]
        public Color defaultColor;
        public Color hoverColor, pressedColor;

        public UnityEvent clickEvent;

        private void Awake()
        {
            boxCol = GetComponent<BoxCollider>();
            if (GetComponent<SpriteRenderer>() != null)
            {
                spriteRend = GetComponent<SpriteRenderer>();
                boxCol.size = new Vector3(spriteRend.size.x, spriteRend.size.y, 0.1f);
                spriteRend.color = defaultColor;
            }
            else
                image = GetComponent<Image>();

            ChangeColor(defaultColor);

        }

        private void OnEnable()
        {
            GetComponent<VRInteractableItem>().OnOver += OnGazeOver;
            GetComponent<VRInteractableItem>().OnOut += OnGazeOut;
            GetComponent<VRInteractableItem>().OnRingComplete += OnRingComplete;
        }

        private void OnDisable()
        {
            GetComponent<VRInteractableItem>().OnOver -= OnGazeOver;
            GetComponent<VRInteractableItem>().OnOut -= OnGazeOut;
            GetComponent<VRInteractableItem>().OnRingComplete -= OnRingComplete;
        }

        private void OnGazeOver()
        {
            ChangeColor(hoverColor);
        }

        private void OnGazeOut()
        {
            ChangeColor(defaultColor);
        }

        private void OnRingComplete()
        {
            StartCoroutine(PressedAnimation());
            clickEvent.Invoke();
        }

        IEnumerator PressedAnimation()
        {
            ChangeColor(pressedColor);
            yield return new WaitForSeconds(0.5f);
            ChangeColor(defaultColor);
        }

        private void ChangeColor(Color color)
        {
            if (spriteRend != null)
                spriteRend.color = color;
            else
                image.color = color;
        }
    }
}