using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
using Debug = UnityEngine.Debug;

public class BoxThrowTrigger : MonoBehaviour
{
    public GameObject TargetObject;

    public UnityEvent OnThrowIn;
    
    private void OnTriggerEnter(Collider other)
    {
        if (TutorialManager.Instance.GetIsTutorialFinished())
            return;
        
        if (other.gameObject ==TargetObject)
        {
            Debug.Log("finished");
            OnThrowIn.Invoke();
        }
        
    }
}
