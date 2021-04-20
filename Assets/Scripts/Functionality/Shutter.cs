using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : MonoBehaviour
{

    public float speed = 1;
    bool activated = false;
    private string direction = "up";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            Debug.Log("Activated");
            // if position has reached top or bottom flip direction
            if (transform.position.y > 2)
            {
                direction = "down";
                Debug.Log("Should move down now");
            }
            else if (transform.position.y < 1.5)
            {
                direction = "up";
            }

            // move shutter in direction defined
            if (direction == "up")
            {
                transform.Translate(Vector3.up * (Time.deltaTime * speed), Space.World);
            }
            else if (direction == "down")
            {
                transform.Translate(Vector3.down * (Time.deltaTime * speed), Space.World);
            }
        }
    }

    public void Movement()
    {
        activated = ! activated;
    }
}
