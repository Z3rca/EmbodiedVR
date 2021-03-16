using System;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor;
using UnityEngine;
using Valve.VR;

public class VRMovement : MonoBehaviour
{

    public GameObject Body;

    public GameObject Head;

    public GameObject Camera;

    public SteamVR_Action_Vector2 Input;
    public SteamVR_ActionSet actionSetEnable;
    
    private Vector2 movementInput;
    public GameObject Orientation;

    private Quaternion targetRotation;
    private void Awake()
    {
        actionSetEnable.Activate();   
        movementInput = new Vector2();
    }

    // Start is called before the first frame update
    void Start()
    {
        Orientation = Camera;
    }


    private void Update()
    {
        movementInput = Input.GetAxis(SteamVR_Input_Sources.Any);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Head.transform.position = Body.transform.position;
        
        
        
        targetRotation= Quaternion.LookRotation(Orientation.transform.forward);
        Vector3 eulerRotation = new Vector3();
        
        eulerRotation= Vector3.ProjectOnPlane(targetRotation.eulerAngles, Vector3.forward);
        eulerRotation.x = 0f;
        targetRotation = Quaternion.Euler(eulerRotation);

        
    }

   
    public Vector2 GetCurrentInput()
    {
        return movementInput;
    }
    

    public Quaternion GetOrientation()
    {
        return targetRotation;
    }
}
