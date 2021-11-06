using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using RootMotion.FinalIK;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BodyScaleCalibration : MonoBehaviour
{
    
    [SerializeField] private VRIKAvatarScaleCalibrationSteamVR ikScaleCalibration;
    public SteamVR_Action_Boolean CalibrationButton;
    [SerializeField] private GameObject BodyObject;
    
    [SerializeField] private GameObject ScreenOverlay;
    private bool _wasPressed;

    private bool _isCalibrating;

    private Dictionary<string, Mesh> MeshDictionary;

    private Hand _leftHand;
    private Hand _rightHand;


    // Start is called before the first frame update
    void Start()
    {
        MeshDictionary = new Dictionary<string, Mesh>();
        Debug.Assert(ikScaleCalibration!=null, "No ik calibration found");
        Debug.Assert(BodyObject,"no Body Object set in BodyScaleCalibration");
        ikScaleCalibration.enabled = false;
        CalibrationButton.AddOnStateDownListener(ButtonPress,SteamVR_Input_Sources.Any);
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    
    public void InitializeCalibration()
    {
        ikScaleCalibration.enabled = true;
        _isCalibrating = true;
    }

    public void ShowBody(bool state)
    {
        if (!BodyObject.activeInHierarchy)
            return;
        
        SkinnedMeshRenderer[] skinnedMeshRenderers = BodyObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in skinnedMeshRenderers)
        {
            if (!state)
            {
                MeshDictionary.Add(renderer.name,renderer.sharedMesh);
                renderer.sharedMesh = null;
            }
            else
            {
                if(MeshDictionary.ContainsKey(renderer.name))
                {
                    renderer.sharedMesh = MeshDictionary[renderer.name];
                }
            }
           
        }
    }

    private void OnDisable()
    {
        CalibrationButton.RemoveOnStateDownListener(ButtonPress,SteamVR_Input_Sources.Any);
    }

    private void ButtonPress(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        _wasPressed = true;
        _isCalibrating = false;
    }

    public void EnableScreen(bool state)
    {
        ScreenOverlay.SetActive(state);
    }

    public bool ButtonWasPressed()
    {
        return _wasPressed;
    }



    
}
