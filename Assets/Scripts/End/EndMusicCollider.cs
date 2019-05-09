using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMusicCollider : MonoBehaviour
{
    public LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (!playerLayer.Matches(other.gameObject.layer)) return;
        
        PlayerController.instance.currentMusic2.FadeOut(2f);
    }
    
}
