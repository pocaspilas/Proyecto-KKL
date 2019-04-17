using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mati36.UI;
using UnityEngine.UI;

namespace Mati36.VR
{
    public class VRGaze : MonoBehaviour
    {
        static public VRGaze currentVRCamera;
        public float gazeDistance;

        [Header("Reticle")]
        public SpriteRenderer reticle;
        public float minReticleDist = 1;
        [Range(0.01f, 1f)]
        public float reticleSize;
        public bool reticleUsingNormals;
        public Color reticleColor;

        [Header("Gaze Ring")]
        public LoadingRing ring;
        public float ringFillTime;

        [Header("Layer")]
        public LayerMask interactableLayer;

        [Header("Camera")]
        [Range(0f, 5f)]
        public float cameraTurnSpeed = 1;

        [Header("Label")]
        public Text itemLabelText;

        private RaycastHit gazeHit;
        private Ray cameraRay;

        private VRInteractableItem currentObjectGazed = null;

        public bool IsGazingSomething { get { return currentObjectGazed != null; } }

        private void OnEnable()
        {
            ring.OnFillComplete += GazeRingCompletedFill;
            currentVRCamera = this;
            reticle.color = reticleColor;
        }

        private void OnDisable()
        {
            ring.OnFillComplete -= GazeRingCompletedFill;
        }

        private void Start()
        {
            reticle.transform.position = transform.position + (minReticleDist * transform.forward);
            reticle.transform.localScale = Vector3.one * reticleSize * minReticleDist;
            Cursor.visible = false;
            itemLabelText.text = "";
        }

        VRInteractableItem gazedItem;

        void Update()
        {
#if UNITY_EDITOR
            var hor = Input.GetAxis("Horizontal");
            var vert = Input.GetAxis("Vertical");

            if (vert != 0)
                transform.Rotate(new Vector3(40 * -vert * cameraTurnSpeed, 0, 0) * Time.deltaTime, Space.Self);
            if (hor != 0)
                transform.Rotate(new Vector3(0, 40 * hor * cameraTurnSpeed, 0) * Time.deltaTime, Space.World);
#endif
            cameraRay = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(cameraRay, out gazeHit, gazeDistance, interactableLayer))
            {
                reticle.transform.position = transform.position + (Vector3.Distance(gazeHit.point, transform.position) * transform.forward);
                reticle.transform.localScale = Vector3.one * reticleSize * Vector3.Distance(gazeHit.point, transform.position);

                if (reticleUsingNormals)
                    reticle.transform.rotation = Quaternion.FromToRotation(Vector3.forward, gazeHit.normal);

                gazedItem = gazeHit.collider.GetComponent<VRInteractableItem>();
                if (gazedItem == null) return;

                if (gazedItem != currentObjectGazed)
                {
                    if (currentObjectGazed != null)
                        currentObjectGazed.GazeOut();

                    currentObjectGazed = gazedItem;
                    currentObjectGazed.GazeOver();

                    if (currentObjectGazed.usesGazeRing)
                        ring.StartFill(ringFillTime);
                    else
                        ring.StopFill();

                    itemLabelText.text = currentObjectGazed.itemName;
                }
            }
            else
            {
                if (currentObjectGazed != null)
                {
                    reticle.transform.position = transform.position + (minReticleDist * transform.forward);
                    reticle.transform.localScale = Vector3.one * reticleSize * minReticleDist;
                    currentObjectGazed.GazeOut();
                    currentObjectGazed = null;

                    ring.StopFill();

                    if (reticleUsingNormals)
                        reticle.transform.forward = transform.forward;

                    itemLabelText.text = "";
                }
            }

        }

        private void GazeRingCompletedFill()
        {
            if (currentObjectGazed != null && currentObjectGazed.usesGazeRing)
            {
                currentObjectGazed.GazeComplete();
            }
        }

        public void EnableReticle()
        {
            reticle.color = reticleColor;
        }

        public void DisableReticle()
        {
            reticle.color = Color.clear;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * gazeDistance);
        }
    }
}