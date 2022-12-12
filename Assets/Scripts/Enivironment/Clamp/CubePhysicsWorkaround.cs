using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePhysicsWorkaround : MonoBehaviour
{
    private Vector3 posFix;
    private Quaternion rotFix;
    private float originalMass;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        posFix = this.transform.position;
        rotFix = this.transform.rotation;
        _rigidbody = GetComponent<Rigidbody>();

        originalMass = _rigidbody.mass;
        _rigidbody.mass = originalMass * 2000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        this.transform.GetComponent<Rigidbody>().position = posFix;
        this.transform.GetComponent<Rigidbody>().rotation = rotFix;
    }

    private void OnDestroy()
    {
        _rigidbody.mass = originalMass;
    }
}
