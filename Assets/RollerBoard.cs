using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerBoard : MonoBehaviour
{

    public float speed = 1.1f;
    public float range = 0.01f;

    private float originalZ;
    
    public string direction = "up";
    
    // Start is called before the first frame update
    void Start()
    {
        originalZ = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > originalZ + range)
        {
            direction = "down";
            //      Debug.Log("Should move down now");
        }
        else if (transform.position.z < originalZ - range)
        {
            direction = "up";
        }

        // move shutter in direction defined
        if (direction == "up")
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * speed), Space.World);
        }
        else if (direction == "down")
        {
            transform.Translate(Vector3.back * (Time.deltaTime * speed), Space.World);
        }
    }
}
