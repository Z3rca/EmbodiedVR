using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class RemoteVR : MonoBehaviour
{

    public Player Player;
    public GameObject LocalHMD;
    public GameObject LocalLeft;
    public GameObject LocalRight;
    
    public GameObject RemoteHMD;

    public GameObject RemoteLeft;

    public GameObject RemoteRight;

    public GameObject RemoteFootPositon;


    private bool isFirstPerson;
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
        Vector3 feetpositon = Player.transform.InverseTransformPoint(Player.feetPositionGuess); 
        
        //Debug.Log(feetpositon);
        
        RemoteHMD.transform.localPosition = LocalHMD.transform.localPosition;
        RemoteHMD.transform.localRotation = LocalHMD.transform.localRotation;

        RemoteLeft.transform.localPosition = LocalLeft.transform.localPosition;
        RemoteLeft.transform.localRotation = LocalLeft.transform.localRotation;

        RemoteRight.transform.localPosition = LocalRight.transform.localPosition;
        RemoteRight.transform.localRotation = LocalRight.transform.localRotation;

        RemoteFootPositon.transform.localPosition = feetpositon;
    }

    private void LateUpdate()
    {
        
    }
}
