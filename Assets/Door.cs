using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool isLeftDoor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openDoor()
    {
        if (isLeftDoor)
        {
            // open to left side
            gameObject.transform.Translate(-1,0,0);
        }
        else
        {
            //open to right side
            gameObject.transform.Translate(1,0,0);
        }
    }
}
