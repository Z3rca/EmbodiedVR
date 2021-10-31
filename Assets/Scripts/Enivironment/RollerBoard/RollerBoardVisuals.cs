using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerBoardVisuals : MonoBehaviour
{

    public float speed = 1.1f;
    public float range = 0.01f;

    private float originalZ;

    private bool directionForward;

    private bool moving;
    
    // Start is called before the first frame update
    void Start()
    {
        originalZ = gameObject.transform.position.z;

        
    }

    public void StopMoving()
    {
        moving = false;
    }
    public void MoveRollerBoard()
    {
        
        if (moving)
            return;
        
        moving = true;
        StartCoroutine(MovingRollerboard());
    }

    private IEnumerator MovingRollerboard()
    {
        Debug.Log("rolling rolling");
        while (moving)
        {
            if (transform.position.z > originalZ + range)
            {
                directionForward = true;
            }
            else if (transform.position.z < originalZ - range)
            {
                directionForward = false;
            }
            
            if (directionForward)
            {
                transform.Translate(Vector3.back * (Time.deltaTime * speed), Space.World);
            }
            else
            {
                transform.Translate(Vector3.forward* (Time.deltaTime * speed), Space.World);
            }

            yield return new WaitForFixedUpdate();
        }
    }
    
    // Update is called once per frame

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveRollerBoard();
        }
    }

    private void FixedUpdate()
    {
//        Debug.Log(GetComponent<Rigidbody>().velocity);
    }
}
