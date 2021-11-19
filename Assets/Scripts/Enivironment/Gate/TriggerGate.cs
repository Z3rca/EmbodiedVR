using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{
    public Transform ClosedPosition;
    public Transform OpenPosition;

    public GameObject Gate;
    [SerializeField] private float speed =0.5f;
    
    private bool _open;

    private bool _opening;

    

    public void SetOpeningState(bool state)
    {
        _opening = state;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (_opening)
        {
            if (!_open)
            {
                
                var position = Gate.transform.position;

                if (ClosedPosition.transform.position.y > OpenPosition.transform.position.y)
                {
                    Debug.Log("in");
                    position.y -= speed * Time.deltaTime;
                }
                else
                {
                    position.y += speed * Time.deltaTime;
                }


                position.y = Mathf.Clamp( position.y, OpenPosition.transform.position.y,
                    ClosedPosition.transform.position.y);

                if (Math.Abs(position.y - OpenPosition.transform.position.y) < 0.01f)
                {
                    _open = true;
                }
                Gate.transform.position = position;
                
                
            }

            _opening = false;
        }
    }
}
