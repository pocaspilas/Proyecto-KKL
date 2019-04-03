using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGaze : MonoBehaviour {
    static public VRGaze currentVRCamera;
    public float gazeDistance;

    [Header ("Reticle")]
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

    private RaycastHit gazeHit;
    private Ray cameraRay;

    private VRInteractableItem currentObjectGazed = null;

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
    }

    VRInteractableItem gazedItem;

    void Update ()
    {
#if UNITY_EDITOR
        var hor = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");

        if(vert != 0)
            transform.Rotate(new Vector3(40 * -vert, 0, 0) * Time.deltaTime, Space.Self);
        if(hor != 0)
            transform.Rotate(new Vector3(0, 40 * hor, 0) * Time.deltaTime, Space.World);

        //if (Input.GetKey(KeyCode.W))
        //    transform.Rotate(new Vector3(-40 ,0 ,0) * Time.deltaTime, Space.Self);

        //if (Input.GetKey(KeyCode.S))
        //    transform.Rotate(new Vector3(40, 0, 0) * Time.deltaTime, Space.Self);

        //if (Input.GetKey(KeyCode.D))
        //    transform.Rotate(new Vector3(0, 40, 0) * Time.deltaTime, Space.World);

        //if (Input.GetKey(KeyCode.A))
        //    transform.Rotate(new Vector3(0, -40, 0) * Time.deltaTime, Space.World);
#endif
        cameraRay = new Ray(transform.position, transform.forward);
        
		if(Physics.Raycast(cameraRay, out gazeHit, gazeDistance, interactableLayer))
        {
            reticle.transform.position = transform.position + (Vector3.Distance(gazeHit.point,transform.position) * transform.forward);
            reticle.transform.localScale = Vector3.one * reticleSize * Vector3.Distance(gazeHit.point, transform.position);

            if(reticleUsingNormals)
                reticle.transform.rotation = Quaternion.FromToRotation(Vector3.forward, gazeHit.normal);

            gazedItem = gazeHit.collider.GetComponent<VRInteractableItem>();
            if (gazedItem == null) return;

            if (gazedItem != currentObjectGazed)
            {
                if(currentObjectGazed != null)
                    currentObjectGazed.GazeOut();

                currentObjectGazed = gazedItem;
                currentObjectGazed.GazeOver();

                if(currentObjectGazed.usesGazeRing)
                    ring.StartFill(ringFillTime);
                else
                    ring.StopFill();
            }
        }
        else
        {
            if (currentObjectGazed != null)
            {
                reticle.transform.position = transform.position +  (minReticleDist * transform.forward);
                reticle.transform.localScale = Vector3.one * reticleSize * minReticleDist;
                currentObjectGazed.GazeOut();
                currentObjectGazed = null;

                ring.StopFill();

                if (reticleUsingNormals)
                    reticle.transform.forward = transform.forward;
            }
        }

    }

    private void GazeRingCompletedFill()
    {
        if(currentObjectGazed != null && currentObjectGazed.usesGazeRing)
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
