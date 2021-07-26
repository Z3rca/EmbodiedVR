using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridController : MonoBehaviour
{

    private Quaternion _currentRotation;

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

        _currentRotation = transform.rotation;
    }
    
    private void MoveCharacterController(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 MovementDirection = new Vector3(input.x, 0f, input.y);
            _characterController.MoveCharacter(MovementDirection);
        }
    }

    private void SwitchView()
    {
        Debug.Log("SwitchView");
    }

    private void RotateChracterController(Quaternion rotation)
    {
        
        _currentRotation *= rotation;
        _characterController.RotateCharacter(_currentRotation);
        Debug.Log( rotation);
    }


    private Quaternion GetCurrentRotation()
    {
        return _currentRotation;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
