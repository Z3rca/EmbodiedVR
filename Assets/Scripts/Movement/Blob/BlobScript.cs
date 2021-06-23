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
    public HybridControl hybridControl;
    // Start is called before the first frame update
    void Start()
    {
        hybridControl.NotifyPerspectiveSwitch += OnPerspectiveSwitch;
        
        foreach (var hand in Player.instance.hands)
        {
            Debug.Log(hand);
            if (hybridControl.GetThirdPerson())
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
    private void OnPerspectiveSwitch(object sender, SwitchPerspectiveEventArgs switchPerspectiveEventArgs)
    {
        
        if (switchPerspectiveEventArgs.switchToThirdPerson)
        {
            BlobModel.SetActive(true);
            
            foreach (var hand in Player.instance.hands)
            {
                hand.HideController();
                
                hand.HideSkeleton();
            }
        }
        else
        {
            BlobModel.SetActive(false);
            foreach (var hand in Player.instance.hands)
            {
                hand.ShowController();
                hand.HideSkeleton();
            }
        }
    }
    
}
