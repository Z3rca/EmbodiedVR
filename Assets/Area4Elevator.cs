using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Area4Elevator : MonoBehaviour
{
    public GameObject lowerDoor;
    public GameObject upperDoor;

    public GameObject LowerPosition;
    public GameObject UpperPosition;
    public GameObject Handle;
   

    public GameObject Plattform;

    private Vector3 plattformPosition;
    public float upperBoundary;
    public float lowerBoundary;

    public float speed;

    private float posX;
    private float posZ;
    
    private LinearMapping _linearMapping;
    private Interactable _interactable;
    
    private float _currentLinearMapping = 0f;

    private Quaternion _initialRotation;

    private bool _isHighlighted;
    private bool _isMovingBack;
    private CircularDrive _circularDrive;

    private float _minAngle;
    private float _maxAngle;

    private void Awake()
    {
        _linearMapping = Handle.GetComponent<LinearMapping>();
        _interactable = Handle.GetComponent<Interactable>();
        _circularDrive = Handle.GetComponent<CircularDrive>();
        _initialRotation = Handle.transform.localRotation;
        _minAngle = _circularDrive.minAngle;
        _maxAngle = _circularDrive.maxAngle;
    }

    // Start is called before the first frame update
    private void Start()
    {
        plattformPosition = LowerPosition.transform.position;

        lowerDoor.SetActive(false);
        upperDoor.SetActive(false);

        

    }

    private void Update()
    {
        _isHighlighted = _interactable.isHovering;
        if (!_isHighlighted)
        {
            //Debug.Log("handle " + Handle.transform.localRotations.y);
            
//            Debug.Log(Math.Abs(Mathf.Tan(Handle.transform.localRotation.eulerAngles.y)));
            
            if (Math.Abs(Mathf.Tan(Handle.transform.localRotation.eulerAngles.y)) >= 0.01)
            {
                MoveBackToInitialState();
            }
            
            
        }
        
        
        
        
    }

    private void LateUpdate()
    {
        _currentLinearMapping = _linearMapping.value;
        Debug.Log(_currentLinearMapping);

        if (!_isHighlighted)
        {
            return;
        }
        if (Math.Abs(_currentLinearMapping - 0.5) <= 0.01f)
        {
            Debug.Log("too short");
        }
        else
        {
            if (_currentLinearMapping < 0.5f)
            {
                MoveUpwards();   
            }
        
            if (_currentLinearMapping > 0.5f)
            {
                MoveDownwards();   
            }
        }

        
        _currentLinearMapping = 0.5f;
       


        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        Plattform.transform.position = plattformPosition;
        
        
        Debug.Log(_currentLinearMapping);

        if (!_isHighlighted)
        {
            return;
        }
        if (Math.Abs(_currentLinearMapping - 0.5) <= 0.01f)
        {
            Debug.Log("too short");
        }
        else
        {
            if (_currentLinearMapping < 0.5f)
            {
                MoveUpwards();   
            }
        
            if (_currentLinearMapping > 0.5f)
            {
                MoveDownwards();   
            }
        }

        
        _currentLinearMapping = 0.5f;   
        
        
    }

    public void MoveUpwards()
    {
        Debug.Log("MOVE UPWARD");
        if (plattformPosition.y <= UpperPosition.transform.position.y)
        {
            lowerDoor.SetActive(true);
            
            plattformPosition.y += speed * Time.deltaTime;
        }
        else
        {
            upperDoor.SetActive(false);
        }
        
    }

    public void MoveDownwards()
    {
        if (gameObject.transform.position.y >= LowerPosition.transform.position.y)
        {
            upperDoor.SetActive(true);
            
            var posY = gameObject.transform.position.y;
            
            posY -= speed * Time.deltaTime;
            
            // posY =  Mathf.Clamp(posY, (lowerBoundary), (upperBoundary));

            gameObject.transform.position = new Vector3(posX, posY, posZ);
        }
        else
        {
            lowerDoor.SetActive(false);
        }
    }


    public void MoveBackToInitialState()
    {
        if (_isMovingBack)
            return;
        _isMovingBack = true;
        Debug.Log("moving back");
        StartCoroutine(MoveHandleToInitialPosition());
    }

    private IEnumerator MoveHandleToInitialPosition()
    {
        while (Quaternion.Angle(Handle.transform.rotation, _initialRotation) >= 0.00f || !_isHighlighted)
        {
            Handle.transform.localRotation = Quaternion.Lerp(Handle.transform.localRotation,_initialRotation,Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        _isMovingBack = false;
    }


}
