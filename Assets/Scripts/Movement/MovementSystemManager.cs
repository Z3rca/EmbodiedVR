using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.AI;

public class MovementSystemManager : MonoBehaviour
{
    //combines the data of the input and the information gathered from the Navmesh agent,to combine and control it easier
    public static MovementSystemManager Instance { get; private set; }


    public GameObject Body;
    public GameObject Head;
    public GameObject Puppet;

    private MovementInput inputSystem;
    private NavMeshMovement _navMeshMovement;
    private NavMeshAgent navmeshAgent;
    private Animator animator;

    private Rigidbody rb;


    private float _currentSpeed;
    private Vector2 _currentInput;
    
    
    public EventHandler<MovementData> InputEvent;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        animator = Puppet.GetComponent<Animator>();
        inputSystem = Body.GetComponent<MovementInput>(); //TODO move this to a more suiting location
        _navMeshMovement = Body.GetComponent<NavMeshMovement>();
        navmeshAgent = Body.GetComponent<NavMeshAgent>();
        rb = Body.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      // Debug.Log(animator.speed);
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        MovementData movementData= new MovementData();
        movementData.input =inputSystem.GetMovementInput();;
        movementData.WorldPosition = _navMeshMovement.GetCurrentPositionOnGround();
        movementData.RotationInWorld = _navMeshMovement.GetCurrentRotation();
        movementData.speed = _navMeshMovement.GetCurrentSpeed();
        
        InputEvent?.Invoke(this,movementData);
    }
}
