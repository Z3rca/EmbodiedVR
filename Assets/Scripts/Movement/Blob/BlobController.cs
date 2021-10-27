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
    // Start is called before the first frame update
    void Start()
    {
        hybridController.OnNotifyPerspectiveSwitchObservers += OnPerspectiveSwitch;
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private  void OnControllerLoaded()
    {
        ModelCounter++;

        if (ModelCounter == 2)
        {
            
        }
    }

    public void Initialize()
    {
        foreach (var hand in Player.instance.hands)
        {
            foreach (var renderModel in hand.renderModels)
            {
                renderModel.displayControllerByDefault = true;
                renderModel.displayHandByDefault = false;
            }
        }

    }


    public void DeactivateHandsOnStartWorkaround(bool thirdperson)
    {
        StartCoroutine(DeactivateHandsWithFade(thirdperson));
    }

    private IEnumerator DeactivateHandsWithFade(bool thirdperson)
    {
        Debug.Log("got here");
        yield return new WaitForSeconds(1f);
        foreach (var hand in Player.instance.hands)
        {
            hand.HideSkeleton();
            hand.HideController(false);
        }
        
        

    }
    
    public void EnabledControllers(bool state)
    {
        if (state)
        {
            foreach (var hand in Player.instance.hands)
            {
                foreach (var renderModel in hand.renderModels)
                {
                    renderModel.SetHandVisibility(false);
                    renderModel.SetControllerVisibility(true);
                }
               // Debug.Log(hand);
               // hand.HideSkeleton(true);
              //  hand.ShowController(true);
            }
            
            
        }
        else
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
        }
        
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
