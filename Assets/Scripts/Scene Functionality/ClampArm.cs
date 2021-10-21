using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class ClampArm : MonoBehaviour
{
    private bool open = false;
    public UnityEvent onClampOpen;
    public GameObject toUnlock;

    // Start is called before the first frame update
    private void Start()
    {
       // Debug.Log("This should mean that console output is working");
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (!open & transform.position.z > 0.0037)
        {
            Debug.Log("Clamp is now open and ball shoul dbe interactable");
            open = true;
            onClampOpen.Invoke();
            Destroy(toUnlock.GetComponent<IgnoreHovering>());
        }
    }
    
}
