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
            if (!thirdperson)
            {
                hand.ShowController();
            }
        }
        
        

    }
    
    public void EnabledControllers(bool state)
    {
        if (state)
        {
            foreach (var hand in Player.instance.hands)
            {
                hand.ShowController();
                hand.HideSkeleton();
            }
        }
        else
        {
            foreach (var hand in Player.instance.hands)
            {
                hand.HideController();
                hand.HideSkeleton();
            }
        }
       
    }
    private void OnPerspectiveSwitch(bool state)
    {
        if (state)
        {
            BlobModel.GetComponent<MeshRenderer>().enabled = true;
            EnabledControllers(!state);
        }
        else
        {
            BlobModel.GetComponent<MeshRenderer>().enabled = false;
            EnabledControllers(!state);
        }
    }
    
}
