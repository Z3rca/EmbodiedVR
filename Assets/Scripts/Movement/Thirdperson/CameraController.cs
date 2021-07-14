using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR;

public class CameraController : MonoBehaviour
{
    public GameObject RotationAxis;
    public GameObject SteamVRPlayer;
    public GameObject CameraArm;
    
    public float cameraDistance;

    private bool isThirdPerson;

    private Quaternion _targetRotation;

    public GameObject Offset;

    public float speed= 5;

    private bool rotationChanged;

    [SerializeField] private StencilWallDection stecilWallDectector;


    // Start is called before the first frame update

    private void Start()
    {
        isThirdPerson = GetComponent<HybridControl>().GetThirdPerson();
        SwitchPerspective();
    }

    // Update is called once per frame
    void Update()
    {
        RotationAxis.transform.rotation = Quaternion.Lerp(RotationAxis.transform.rotation, Quaternion.Euler(_targetRotation.eulerAngles),Time.deltaTime* speed );
            
        SteamVRPlayer.transform.position = CameraArm.transform.position;
    }
    
    public void SwitchPerspective()
    {
        if (!isThirdPerson)
        {
            CameraArm.transform.localPosition = Vector3.zero;
            if (stecilWallDectector != null)
            {
                stecilWallDectector.IgnoreMask(true);
            }
        }
        else
        {
            CameraArm.transform.localPosition= Vector3.back*cameraDistance;
            if (stecilWallDectector != null)
            {
                stecilWallDectector.IgnoreMask(false);
            }
        }
    }

    public void SetThirdPerson(bool state)
    {
        isThirdPerson = state;
    }


    public void AddOffset(Vector3 offset)
    {
        Offset.transform.localPosition += offset;
    }
    
    
    public void RotateCamera(Quaternion rotation)
    {
        _targetRotation = rotation;
    }


    
}
