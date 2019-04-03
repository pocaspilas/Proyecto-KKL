using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(VRInteractableItem))]
public class MenuBase : MonoBehaviour {
    public LoadingRing ring;
    public UnityEvent fullRingEvent;
    public float ringFillTime;

    private void OnEnable()
    {
        GetComponent<VRInteractableItem>().OnOver += GazeOverMenu;
        GetComponent<VRInteractableItem>().OnOut += GazeOutMenu;
        ring.OnFillComplete += FullRing;
    }

    private void OnDisable()
    {
        GetComponent<VRInteractableItem>().OnOver -= GazeOverMenu;
        GetComponent<VRInteractableItem>().OnOut -= GazeOutMenu;
        ring.OnFillComplete -= FullRing;
    }

    private void GazeOverMenu()
    {
        ring.StartFill(ringFillTime);
    }

    private void GazeOutMenu()
    {
        ring.StopFill();
    }

    private void FullRing()
    {
        fullRingEvent.Invoke();
    }
}
