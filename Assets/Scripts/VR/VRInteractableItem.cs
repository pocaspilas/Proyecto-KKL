using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VRInteractableItem : MonoBehaviour {

    private bool _isOver;
    private bool _isClicked;
    public bool IsOver { get { return _isOver; } }

    public event System.Action OnOver = delegate { };
    public event System.Action OnOut = delegate { };
    public event System.Action OnClick = delegate { };
    public event System.Action OnClickDown = delegate { };
    public event System.Action OnClickUp = delegate { };
    public event System.Action OnRingComplete = delegate { };


    public void GazeOver()
    {
        _isOver = true;
        OnOver();
    }

    public void GazeOut()
    {
        _isOver = false;
        //if (_isClicked)
        //{
        //    OnClickUp();
        //    _isClicked = false;
        //}
        OnOut();
    }

    private void Update()
    {
        if(_isOver)
        {
            if(Input.GetMouseButton(0))
            {
                OnClick();
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                OnClickDown();
                if (!_isClicked)
                    _isClicked = true;
            }
            else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            {
                OnClickUp();
                if(_isClicked)
                    _isClicked = false;
            }
        }
        else
        {
            if (_isClicked && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)))
            {
                OnClickUp();
                _isClicked = false;
            }

        }
    }

    public bool usesGazeRing;

    public void GazeComplete()
    {
        OnRingComplete();
    }
}
