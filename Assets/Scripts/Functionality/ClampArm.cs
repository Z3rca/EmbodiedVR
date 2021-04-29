using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ClampArm : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void unlockClamp()
    {
        GetComponent<Interactable>().enabled=true;
        GetComponent<LinearDrive>().enabled=true;
    }
    
    void lockClamp()
    {
        GetComponent<Interactable>().enabled=false;
        GetComponent<LinearDrive>().enabled=false;
    }
}
