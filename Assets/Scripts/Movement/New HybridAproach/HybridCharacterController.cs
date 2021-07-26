using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCharacterController : MonoBehaviour
{
    public float ForwardSpeed;
    public float SideWaySpeed;
    
    private float _speedFactor;
    private CharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        _speedFactor = 1f;
        
        _characterController= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MoveCharacter(Vector3 movementDirection)
    {
        
        var xDirection = transform.right;
        var zDirection = transform.forward;
        Vector3 move = (xDirection * (movementDirection.x * SideWaySpeed*_speedFactor) +
                        (movementDirection.y>=0? 
                            zDirection * (movementDirection.z * ForwardSpeed*_speedFactor): 
                            zDirection * (movementDirection.z * SideWaySpeed*_speedFactor) ));
        
        _characterController.Move(move*Time.deltaTime);
    }
    
    
    
    
    
    public void SetSpeedFactor(float percentage)
    {
        _speedFactor = percentage;
    }


    public void RotateCharacter(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }


    public Vector3 GetCharacterPosition()
    {
        return this.transform.position;
    }
}
