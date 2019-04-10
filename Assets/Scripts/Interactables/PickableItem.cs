using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(VRInteractableItem))]
public class PickableItem : MonoBehaviour
{
    private Animator ani;
    private VRInteractableItem _interactable;
    private Renderer[] _rend;

    MaterialPropertyBlock mpb;
    Material sharedMat;

    public Material outlineMat;

    private void Awake()
    {
        _interactable = GetComponent<VRInteractableItem>();
        _rend = GetComponentsInChildren<Renderer>();
        sharedMat = _rend[0].sharedMaterial;
        ani = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _interactable.OnOver += Interactable_OnOver;
        _interactable.OnOut += Interactable_OnOut;
        _interactable.OnRingComplete += Interactable_OnRingComplete;
    }

    private void OnDisable()
    {
        _interactable.OnOver -= Interactable_OnOver;
        _interactable.OnOut -= Interactable_OnOut;
        _interactable.OnRingComplete -= Interactable_OnRingComplete;
    }

    private void Interactable_OnOver()
    {
        foreach (var r in _rend)
            r.material = outlineMat;
    }

    private void Interactable_OnOut()
    {
        foreach (var r in _rend)
            r.sharedMaterial = sharedMat;
    }

    private void Interactable_OnRingComplete()
    {
        PlayerController.instance.OnItemCollect();
        ani.Play("Object_Pick", 0, 0f);
        enabled = false;
        //Destroy(gameObject);
    }
}
