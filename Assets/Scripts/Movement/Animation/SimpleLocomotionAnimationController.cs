using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleLocomotionAnimationController: MonoBehaviour
{
    private CharacterController characterController;

    private InputData _inputData;
    public MovementInput input;

    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        characterController = input.GetComponent<CharacterController>();
        input.InputEvent += ReadInputAsAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = camera.transform.position;

    }


    private void ReadInputAsAnimation(object sender, InputData inputData)
    {
        Debug.Log("received input " + inputData.input );
        if (inputData.input != Vector2.zero)
        {
            GetComponent<Animator>().SetBool("IsMoving",true);
            GetComponent<Animator>().SetFloat("X", inputData.input.x);
            GetComponent<Animator>().SetFloat("Z", inputData.input.y);
        }
        else
        {
            GetComponent<Animator>().SetBool("IsMoving",false);
        }
    }
}
