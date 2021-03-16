using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.B))
        {
            transform.position += direction*Time.deltaTime;
        }
    }
}
