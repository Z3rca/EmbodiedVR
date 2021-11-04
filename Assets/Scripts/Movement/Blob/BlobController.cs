using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BlobController : MonoBehaviour
{
    private int ModelCounter;
    public GameObject BlobModel;
    public HybridController hybridController;

    public Hand LeftHand;

    public Hand RightHand;
    // Start is called before the first frame update
    void Start()
    {
        hybridController.OnNotifyPerspectiveSwitchObservers += OnPerspectiveSwitch;

        LeftHand = Player.instance.hands[0];
        RightHand = Player.instance.hands[1];
        
       
    }
    // Update is called once per frame
    void Update()
    {
        LeftHand.SetVisibility(false);
        RightHand.SetVisibility(false);
    }

    private  void OnControllerLoaded()
    {
        ModelCounter++;

        if (ModelCounter == 2)
        {
            
        }
    }

    private void OnEnable()
    {
        
    }

    public void Initialize()
    {
        foreach (var hand in Player.instance.hands)
        {
            foreach (var renderModel in hand.renderModels)
            {
                renderModel.displayControllerByDefault = false;
                renderModel.displayHandByDefault = false;
            }
        }
        
        foreach (var hand in Player.instance.hands)
        {
            foreach (var renderModel in hand.renderModels)
            {
                renderModel.SetHandVisibility(false, true);
                renderModel.SetControllerVisibility(false,true);
            }
            //hand.HideController();
            // hand.HideSkeleton();
        }

    }


    public void DeactivateHandsOnStartWorkaround(bool thirdperson)
    {
        StartCoroutine(DeactivateHandsWithFade(thirdperson));
    }

    private IEnumerator DeactivateHandsWithFade(bool thirdperson)
    {
        Debug.Log("got here");
        yield return new WaitForSeconds(2f);
        /*foreach (var hand in Player.instance.hands)
        {
            foreach (var renderModel in hand.renderModels)
            {
                renderModel.SetHandVisibility(false);
                renderModel.SetControllerVisibility(false);
            }
        }
        */
        Debug.Log("controllers there");
        EnabledControllers(true);


    }
    
    public void EnabledControllers(bool state)
    {
        
        
        foreach (var hand in Player.instance.hands)
        {
            foreach (var renderModel in hand.renderModels)
            {
                renderModel.SetHandVisibility(false);
                renderModel.SetControllerVisibility(false);
            }
            //hand.HideController();
            // hand.HideSkeleton();
        }
        
        if(!ExperimentManager.Instance.isTutorialRunning())
            hybridController.ShowControllers(state);
       
    }
    private void OnPerspectiveSwitch(bool state)
    {
        if (state)
        {
            Debug.Log("switch");
            BlobModel.GetComponent<MeshRenderer>().enabled = true;
            EnabledControllers(false);
        }
        else
        {
            BlobModel.GetComponent<MeshRenderer>().enabled = false;
            EnabledControllers(true);
        }
    }
    
}
