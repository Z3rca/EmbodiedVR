using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : MonoBehaviour
{
    public GameObject shutterOrigin;
    public GameObject UpperPosition;
    public GameObject LowerPosition;
    public float speed = 1;
    bool activated = false;
    private bool directionUp;
    
    private bool _switch;
    private bool _recovering;
    
    // Start is called before the first frame update

    private void Start()
    {
        shutterOrigin.transform.position = LowerPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
          //  Debug.Log("Activated");
            // if position has reached top or bottom flip direction
            if (shutterOrigin.transform.position.y>=UpperPosition.transform.position.y)
            {
                directionUp = false;
                //      Debug.Log("Should move down now");
            }
            
            if (shutterOrigin.transform.position.y<=LowerPosition.transform.position.y)
            {
                directionUp = true;
            }

            // move shutter in direction defined
            if (directionUp)
            {
                shutterOrigin.transform.Translate(Vector3.up * (Time.deltaTime * speed), Space.World);
            }
            else if (!directionUp)
            {
                shutterOrigin.transform.Translate(Vector3.down * (Time.deltaTime * speed), Space.World);
            }
        }
    }
    
    
    

    public void StartMoving(bool state)
    {
        activated = state;
    }

    public void SwitchMoving()
    {
        if (_recovering)
            return;
        _recovering = true;
        _switch = !_switch;
        StartMoving(_switch);
    }

    public void ButtonRecovery()
    {
        StartCoroutine(DelayedRecovery(2f));
    }
    private IEnumerator DelayedRecovery(float time)
    {
        yield return new WaitForSeconds(time);

        _recovering = false;

    }
}
