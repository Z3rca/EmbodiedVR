using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class ClampArm : MonoBehaviour
{
    private bool _open = false;
    public UnityEvent onClampOpen;
    public GameObject toUnlock;
    public Transform ThresholdPosition;

    // Start is called before the first frame update
    private void Start()
    {
        toUnlock.GetComponent<Rigidbody>().useGravity = false;
        //toUnlock.GetComponent<Rigidbody>().isKinematic = true;
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_open & Vector3.Distance(this.transform.position,ThresholdPosition.transform.position)>0.02f)
        {
            _open = true;
            onClampOpen.Invoke();
            var otherIgnoreHovering = toUnlock.GetComponent<IgnoreHovering>();
            var physicsWorkaround = toUnlock.GetComponent<CubePhysicsWorkaround>();
            toUnlock.GetComponent<Rigidbody>().useGravity = true;
            //toUnlock.GetComponent<Rigidbody>().isKinematic = false;
            toUnlock.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            Destroy(otherIgnoreHovering);
            Destroy(physicsWorkaround);
            
        }
    }

    public void SetOpen()
    {
        _open = true;
        var ignoreHovering = GetComponent<IgnoreHovering>();
        Destroy(ignoreHovering);
        
    }
    
    
}
