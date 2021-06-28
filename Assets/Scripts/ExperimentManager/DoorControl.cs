using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject Door;
    public GameObject Door2;


    public void CloseDoor()
    {
        Door.gameObject.SetActive(true);
        Door2.gameObject.SetActive(true);
    }
    
    
    public void OpenDoor()
    {
        Door.gameObject.SetActive(true);
        Door2.gameObject.SetActive(true);
    }
}
