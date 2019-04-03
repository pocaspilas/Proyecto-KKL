using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
	
	// Update is called once per frame
	void Update () {
        var horAxis = Input.GetAxis("Horizontal");
        var vertAxis = Input.GetAxis("Vertical");

        if (vertAxis != 0)
            transform.Rotate(new Vector3(40 * -vertAxis, 0, 0) * Time.deltaTime, Space.Self);
        if (horAxis != 0)
            transform.Rotate(new Vector3(0, 40 * horAxis, 0) * Time.deltaTime, Space.World);
    }
}
