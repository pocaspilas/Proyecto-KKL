using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Camera currentCam;

    private void Awake()
    {
        currentCam = Camera.main;
    }

    private void Update()
    {
        transform.forward = -(currentCam.transform.position - transform.position).normalized;
    }
}
