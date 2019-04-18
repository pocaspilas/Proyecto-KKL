using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mati36.VR;
using Mati36.Sound;

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
    private Material outlineMatInstance;
    
    private bool picked;

    public AnimationCurve scaleAnimation;
    [Header("Sounds")]
    public SoundAsset pickSound;

    private void Awake()
    {
        _interactable = GetComponent<VRInteractableItem>();
        _rend = GetComponentsInChildren<Renderer>();
        sharedMat = _rend[0].sharedMaterial;
        
        ani = GetComponent<Animator>();
        picked = false;
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
        if (picked) return;

        if(outlineMatInstance == null)
        {
            outlineMatInstance = new Material(outlineMat);
            outlineMatInstance.mainTexture = sharedMat.mainTexture;
        }
        foreach (var r in _rend)
            r.material = outlineMatInstance;
    }

    private void Interactable_OnOut()
    {
        if (picked) return;
        foreach (var r in _rend)
            r.sharedMaterial = sharedMat;
    }

    private void Interactable_OnRingComplete()
    {
        StartCoroutine(PickAnimation());
        PlayerController.instance.OnItemCollect();
    }

    IEnumerator PickAnimation()
    {
        SoundManager.PlayOneShotSound(pickSound);
        picked = true;
        _interactable.enabled = false;
        float time = 0;
        Vector3 initialPos = transform.position;
        Vector3 initialScale = transform.localScale;
        while(time < 1)
        {
            transform.position = BezierUtility.SimpleBezier(initialPos, PlayerController.PushkePosition, time, 5f);
            transform.localScale = initialScale * scaleAnimation.Evaluate(time);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
