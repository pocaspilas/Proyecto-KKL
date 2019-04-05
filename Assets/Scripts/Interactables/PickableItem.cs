using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VRInteractableItem))]
public class PickableItem : MonoBehaviour
{
    VRInteractableItem _interactable;
    Renderer[] _rend;

    MaterialPropertyBlock mpb;
    Material sharedMat;

    public Material outlineMat;

    private void Awake()
    {
        _interactable = GetComponent<VRInteractableItem>();
        _interactable.OnOver += Interactable_OnOver;
        _interactable.OnOut += Interactable_OnOut;
        _interactable.OnRingComplete += Interactable_OnRingComplete;

        _rend = GetComponentsInChildren<Renderer>();
        sharedMat = _rend[0].sharedMaterial;
    }
    
    private void Interactable_OnOver()
    {
        foreach(var r in _rend)
            r.material = outlineMat;
    }

    private void Interactable_OnOut()
    {
        foreach(var r in _rend)
            r.sharedMaterial = sharedMat;
    }

    private void Interactable_OnRingComplete()
    {
        Destroy(gameObject);
    }
}
