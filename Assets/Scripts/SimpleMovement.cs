using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement: MonoBehaviour
{
    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(Vector3.forward*Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(Vector3.right*Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(Vector3.left*Time.deltaTime);
        }
    }
}
