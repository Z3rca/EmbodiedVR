using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RollerboardController : MonoBehaviour
{
    private RollerBoardVisuals _visuals;

    public UnityEvent OnTriggerActivate;
    public UnityEvent OnTriggerDeactivated;

    private void Start()
    {
        _visuals = GetComponent<RollerBoardVisuals>();
    }


    public void StartRolling()
    {
        _visuals.MoveRollerBoard();
    }

    public void StopRolling()
    {
        _visuals.StopMoving();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            other.transform.parent.parent = this.transform;
            OnTriggerActivate.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            other.transform.parent.parent = null;
        }
        OnTriggerDeactivated.Invoke();
    }
    
}
