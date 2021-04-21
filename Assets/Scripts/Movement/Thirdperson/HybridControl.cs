using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridControl : MonoBehaviour
{
    private VRMovement InputController;
    public CameraController cameraController;

    private void Start()
    {
        InputController = GetComponent<VRMovement>();
        //cameraController = GetComponentInChildren<CameraController>();
    }


    private void Update()
    {
        cameraController.RotateCamera(InputController.GetRotation());
    }



    public void SwitchPerspective()
    {
        cameraController.SwitchPerspective();
    }
}
