using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridController : MonoBehaviour
{

    private InputController _inputController;

    private HybridCharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponentInChildren<HybridCharacterController>();
        _inputController = GetComponent<InputController>();
        Debug.Log(_inputController);

        _inputController.OnNotifyControlStickMovedObservers += MoveCharacterController;
        _inputController.OnNotifySwitchButtonPressedObserver += SwitchView;
        _inputController.OnNotifyRotationPerformed += RotateChracterController;
    }


    private void MoveCharacterController(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 MovementDirection = new Vector3(input.x, 0f, input.y);
            _characterController.MoveCharacter(MovementDirection);
            Debug.Log(input);
        }
    }

    private void SwitchView()
    {
        Debug.Log("SwitchView");
    }

    private void RotateChracterController(Quaternion rotation)
    {
        Debug.Log( rotation);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
