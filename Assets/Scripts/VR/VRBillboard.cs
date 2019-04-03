using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBillboard : MonoBehaviour {
    Camera currentCamera;

    private Vector3 smoothVel = new Vector3();
    public float smoothValue = 0.5f;
    public bool onlyHorizontal;

    private void Awake()
    {
        currentCamera = Camera.main;
    }

    private void Update()
    {
        transform.forward = Vector3.SmoothDamp(transform.forward, currentCamera.transform.forward, ref smoothVel, smoothValue);

        if (onlyHorizontal)
            transform.forward = transform.forward.SetY(0);
    }
}
