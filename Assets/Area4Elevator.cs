using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Area4Elevator : MonoBehaviour
{
    public GameObject lowerDoor;
    public GameObject upperDoor;

    public GameObject LowerPosition;
    public GameObject UpperPosition;
    public GameObject Handle;
    public GameObject Plattform;

    public bool startsAtTop;
    
    private Vector3 plattformPosition;
    
    public float speed;

    private float posX;
    private float posZ;
    
    private LinearMapping _linearMapping;
    private Interactable _interactable;
    
    private float _currentLinearMapping = 0f;

    private Quaternion _initialRotation;

    private bool _isHighlighted;
    private bool _isMovingBack;

    private bool _isAttached;

    private bool _isInitialized;

    private bool isMovingUpward;
    private bool isMovingDownward;
    private CircularDrive _circularDrive;

    private SteamVR_Skeleton_Poser poseR;

    private float _minAngle;
    private float _maxAngle;

    private Hand LeftHand;
    private Hand RightHand;
    private LinearDrive _linearDrive;

    private void Awake()
    {
        
            
        _linearMapping = Handle.GetComponent<LinearMapping>();
        _interactable = Handle.GetComponent<Interactable>();
        _circularDrive = Handle.GetComponent<CircularDrive>();

        _linearDrive = Handle.GetComponent<LinearDrive>();
        _initialRotation = Handle.transform.localRotation;
//        _minAngle = _circularDrive.minAngle;
 //       _maxAngle = _circularDrive.maxAngle;
    }


    private void Initialize(object sender, StartExperimentArgs startExperimentArgs)
    {
        LeftHand= ExperimentManager.Instance.GetPlayerController().GetRemoteTransformController().LocalLeft.GetComponent<Hand>();
        RightHand= ExperimentManager.Instance.GetPlayerController().GetRemoteTransformController().LocalLeft.GetComponent<Hand>();
        _isInitialized = true;
        


    }
    // Start is called before the first frame update
    private void Start()
    {
        if (startsAtTop)
        {
            plattformPosition = UpperPosition.transform.position;
        }
        else
        {
            plattformPosition = LowerPosition.transform.position;
        }
        

        //lowerDoor.SetActive(false);
        //upperDoor.SetActive(false);


        ExperimentManager.Instance.startedExperiment += Initialize;
    }

    private void Update()
    {
        if (_isInitialized)
        {
            _isHighlighted = _interactable.isHovering;
            if (!_isAttached)
            {
                //Debug.Log("handle " + Handle.transform.localRotations.y);
            
//            Debug.Log(Math.Abs(Mathf.Tan(Handle.transform.localRotation.eulerAngles.y)));
            
                if (Math.Abs(Mathf.Tan(Handle.transform.localRotation.eulerAngles.y)) >= 0.01)
                {
                    MoveBackToInitialState();
                }
            
            
            }

        }
    }

    private void LateUpdate()
    {
        Plattform.transform.position = plattformPosition;
        
        _currentLinearMapping = _linearMapping.value;

        if (_isAttached)
        {
            _currentLinearMapping = _linearMapping.value;
            if (Math.Abs(_currentLinearMapping - 0.5f) > 0.01)
            {
                if (_currentLinearMapping < 0.5)
                {
                    isMovingUpward = true;
                    isMovingDownward = false;
                }

                if (_currentLinearMapping > 0.5f)
                {
                    isMovingDownward = true;
                    isMovingUpward = false;
                }
            }
            else
            {
                isMovingUpward = false;
                isMovingDownward = false;
            }
        }
        else
        {
            _currentLinearMapping = Mathf.Tan(Handle.transform.localRotation.eulerAngles.y);
            _linearMapping.value = _currentLinearMapping;
            isMovingDownward = false;
            isMovingUpward = false;
        }
        
       


        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isMovingUpward)
        {
            MoveUpwards();
            return;
        }

        if (isMovingDownward)
        {
            MoveDownwards();
            return;
        }
        

    }

    public void MoveUpwards()
    {
        Debug.Log("MOVE UPWARD");
        if (plattformPosition.y <= UpperPosition.transform.position.y)
        {
            lowerDoor.SetActive(true);
            
            plattformPosition.y += speed * Time.fixedDeltaTime;
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
            
            plattformPosition.y -= speed * Time.fixedDeltaTime;
            
            // posY =  Mathf.Clamp(posY, (lowerBoundary), (upperBoundary));
            
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
        while (Quaternion.Angle(Handle.transform.rotation, _initialRotation) >= 0.00f || !_isAttached)
        {
            Handle.transform.localRotation = Quaternion.Lerp(Handle.transform.localRotation,_initialRotation,Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        _isMovingBack = false;
    }


    public void ObjectAttached()
    {
        _isAttached=true;
    }

    public void ObjectDetached()
    {
        _isAttached = false;
    }


}
