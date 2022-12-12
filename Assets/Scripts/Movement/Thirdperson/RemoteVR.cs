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
    public GameObject LocalLeftArm;
    public GameObject LocalRight;
    public GameObject LocalRightArm;
    public GameObject LocalHeadTarget;
    
    public GameObject RemoteHMD;

    public GameObject RemoteLeft;
    public GameObject RemoteLeftArm;

    public GameObject RemoteRight;
    public GameObject RemoteRightArm;

    public GameObject RemoteFootPositon;


    public GameObject Hands;

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

    

    public void SetLocalHands()
    {
        Hands.transform.parent = LocalHeadTarget.transform.parent.parent;
        Hands.transform.localPosition =Vector3.zero;
    }
    
    public void SetRemoteHands()
    {
        Hands.transform.parent = this.transform;
        Hands.transform.localPosition =Vector3.zero;
    }
}
