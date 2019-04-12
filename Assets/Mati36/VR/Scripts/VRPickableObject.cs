using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mati36.VR
{
    [RequireComponent(typeof(Rigidbody), typeof(VRInteractableItem))]
    public class VRPickableObject : MonoBehaviour
    {

        protected bool isPicked = false;
        protected Rigidbody rb;
        protected MeshRenderer meshRend;
        protected VRInteractableItem interactableScript;

        protected Vector3 lastPosition;
        protected Vector3 currentVel;

        public bool usingPhysics = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            meshRend = GetComponent<MeshRenderer>();
            interactableScript = GetComponent<VRInteractableItem>();
            lastPosition = transform.position;

            meshRend.material.SetColor("_ASEOutlineColor", Color.green);
        }

        private void OnEnable()
        {
            interactableScript.OnClickDown += Pick;
            interactableScript.OnClickUp += Drop;
            interactableScript.OnOver += OnOver;
            interactableScript.OnOut += OnOut;
        }

        private void OnDisable()
        {
            interactableScript.OnClickDown -= Pick;
            interactableScript.OnClickUp -= Drop;
            interactableScript.OnOver -= OnOver;
            interactableScript.OnOut -= OnOut;
        }

        protected Vector3 vectorToCamera;

        protected virtual void Update()
        {
            if (isPicked)
            {
                if (!usingPhysics)
                {
                    currentVel = (transform.position - lastPosition) / Time.deltaTime;
                    lastPosition = transform.position;
                }
                else
                {
                    vectorToCamera = VRGaze.currentVRCamera.transform.position + (VRGaze.currentVRCamera.transform.forward * (VRGaze.currentVRCamera.transform.position - transform.position).magnitude) - transform.position;
                    rb.velocity += vectorToCamera * Time.deltaTime * 25;

                    rb.velocity *= 0.9f;
                }
            }
        }

        void OnOver()
        {
            meshRend.material.SetFloat("_ASEOutlineWidth", 0.06f);
        }

        void OnOut()
        {
            if (!isPicked)
                meshRend.material.SetFloat("_ASEOutlineWidth", 0f);
        }

        void Pick()
        {
            if (!usingPhysics)
            {
                transform.parent = VRGaze.currentVRCamera.transform;
                lastPosition = transform.position;
                currentVel = Vector3.zero;
                rb.isKinematic = true;
            }
            isPicked = true;
            rb.useGravity = false;

            meshRend.material.SetColor("_ASEOutlineColor", Color.red);
        }

        void Drop()
        {
            if (!usingPhysics)
            {
                transform.parent = null;
                rb.isKinematic = false;
                rb.AddForce(currentVel, ForceMode.VelocityChange);
            }
            rb.useGravity = true;
            isPicked = false;

            meshRend.material.SetColor("_ASEOutlineColor", Color.green);

            if (!interactableScript.IsOver)
                meshRend.material.SetFloat("_ASEOutlineWidth", 0f);
        }
    }
}