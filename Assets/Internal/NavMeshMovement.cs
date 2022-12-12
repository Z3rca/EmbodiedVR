using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Valve.VR;

public class NavMeshMovement : MonoBehaviour
{
    [SerializeField] private Transform  _hmdTransform;
    [SerializeField] private GameObject  head;
    [SerializeField] private GameObject  Orientation;
    private NavMeshAgent agent;
    [Range(0.0f,15.0f)] public float speed;
    private Vector3 BodyDirection;

    private float _currentSpeed;
    
    private bool freezed;

    private Vector3 oldPos;
    private Vector3 newPos;


    public float SideAndBackFactor = 0.33f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = true;
        oldPos =newPos=  transform.position;
    }

    private void FixedUpdate()
    {
        if(!freezed)
            head.transform.position = this.transform.position;
        
        newPos = transform.position;
        var media =  (newPos - oldPos);
        _currentSpeed = (media / Time.deltaTime).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        
        Quaternion targetRot = Quaternion.LookRotation(Orientation.transform.forward);
        Vector3 eulerRotation = new Vector3();
        
        eulerRotation= Vector3.ProjectOnPlane(targetRot.eulerAngles, Vector3.forward);
        eulerRotation.x = 0f;
        targetRot = Quaternion.Euler(eulerRotation);


        this.transform.rotation = targetRot;
        
        
        newPos = transform.position;
        var media =  (newPos - oldPos);
        _currentSpeed = (media.magnitude / Time.deltaTime);
      
        
        
//        Debug.Log(_currentSpeed);
        
       //agent.angularSpeed = 300;
       oldPos = newPos;
        
        
    }

    private void LateUpdate()
    {
        
    }

    public void Move(Vector2 direction)
    {
        BodyDirection = new Vector3( direction.x,0f,direction.y  );
        BodyDirection =  this.transform.rotation* BodyDirection;
        //Debug.Log("body" + BodyDirection);
        //Debug.Log("head" + direction);
        MoveInToDirection(BodyDirection);
    }

    private void MoveInToDirection(Vector3 direction)
    {
        
        direction = direction * (Time.deltaTime * speed);
        agent.Move(direction);
        
       // _currentSpeed = direction.magnitude;
        agent.velocity = direction;
//        Debug.Log(agent.velocity.z +" " + _currentSpeed);
    }


    private void FreezeMover()
    {
       
    }
    
    
    public void SetToPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        // todo set rotation as well

       FreezeMover();
        transform.position = pos;
        agent.Warp(pos);

        transform.rotation = rot;

        Valve.VR.OpenVR.Compositor.SetTrackingSpace(
            Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

        
        
        //agent.enabled = true;
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    public Vector3 GetCurrentPositionOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(_hmdTransform.position, Vector3.down, out hit))
        {
            return hit.point;
        }
        else
        {
            return _hmdTransform.position;
        }
    }

    public Quaternion GetCurrentRotation()
    {
        return _hmdTransform.rotation;
    }
    
}
