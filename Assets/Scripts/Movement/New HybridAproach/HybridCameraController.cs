﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float cameraDistance;
    [SerializeField] private GameObject CameraArm;
    private Vector3 targetPosition;
    private bool _thirdPersonWasActivated;

    private void Start()
    {
        targetPosition = this.transform.position;

        cameraDistance = CameraArm.transform.localPosition.z;
    }

    public void RotateCamera(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    public void SetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void SwitchPerspective(bool toThirdPerson)
    {
        if (toThirdPerson)
        {
            CameraArm.transform.localPosition = Vector3.forward * cameraDistance;
        }
        else
        {
            CameraArm.transform.localPosition=Vector3.zero;
        }
        
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, Time.deltaTime * speed);
    }
    
    
}
