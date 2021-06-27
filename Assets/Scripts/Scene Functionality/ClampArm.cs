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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!open & transform.position.y > 1.45)
        {
            onClampOpen.Invoke();
            Destroy(toUnlock.GetComponent<IgnoreHovering>());
        }
    }
    
}
