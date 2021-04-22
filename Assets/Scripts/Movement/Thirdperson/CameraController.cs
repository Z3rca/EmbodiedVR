using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR;

public class CameraController : MonoBehaviour
{
    public GameObject RotationAxis;
    public GameObject Camera;
    public GameObject Player;
    public GameObject ThirdPersonPosition;
    public GameObject CameraArm;

    public bool VrObjectOriented;

    public float cameraDistance;

    public VRMovement characterController;

    private bool isThirdPerson=true;

    private Quaternion _targetRotation;

    public float speed= 5;

    private bool rotationChanged;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        
        if (VrObjectOriented)
        {
            Vector3 currentRotation = RotationAxis.transform.rotation.eulerAngles;
            Vector3 eulerRotAxis = characterController.GetRotation().eulerAngles;
            eulerRotAxis.x = 0;
            eulerRotAxis.z = 0;

           //eulerRotAxis.y= Mathf.Lerp(currentRotation.y, eulerRotAxis.y, Time.deltaTime * 2f);
            
            RotationAxis.transform.rotation = Quaternion.Lerp(RotationAxis.transform.rotation, Quaternion.Euler(_targetRotation.eulerAngles),Time.deltaTime* speed );
            
            Player.transform.position = CameraArm.transform.position;
        }
        
    }


    public void SwitchPerspective(bool Fade=true)
    {
        
        if (isThirdPerson)
        {
            isThirdPerson = false;
            CameraArm.transform.localPosition = Vector3.zero;
            
        }
        else
        {
            isThirdPerson = true;
            CameraArm.transform.localPosition= Vector3.back*cameraDistance;
        }
    }


    public void RotateCamera(Quaternion rotation)
    {
        _targetRotation = rotation;
    }


    
}
