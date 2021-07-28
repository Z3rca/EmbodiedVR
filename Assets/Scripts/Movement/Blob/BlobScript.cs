using System.Collections;
using System.Collections.Generic;
using RootMotion.Demos;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BlobScript : MonoBehaviour
{
    private int ModelCounter;
    public GameObject BlobModel;
    public HybridController hybridController;
    // Start is called before the first frame update
    void Start()
    {
        hybridController.OnNotifyPerspectiveSwitchObservers += OnPerspectiveSwitch;
        
        foreach (var hand in Player.instance.hands)
        {
            Debug.Log(hand);
            if (hybridController.IsCurrentlyInThirdperson())
            {
                hand.HideController();
                
                hand.HideSkeleton(); 
            }
            else
            {
                hand.ShowController();
                
                hand.HideSkeleton();
            }
            
        }
        
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
    private void OnPerspectiveSwitch(bool state)
    {
        
        if (state)
        {
            BlobModel.GetComponent<MeshRenderer>().enabled = true;
            //BlobModel.SetActive(true);
            
            foreach (var hand in Player.instance.hands)
            {
                hand.HideController();
                
                hand.HideSkeleton();
            }
        }
        else
        {
            BlobModel.GetComponent<MeshRenderer>().enabled = false;
            foreach (var hand in Player.instance.hands)
            {
                hand.ShowController();
                hand.HideSkeleton();
            }
        }
    }
    
}
