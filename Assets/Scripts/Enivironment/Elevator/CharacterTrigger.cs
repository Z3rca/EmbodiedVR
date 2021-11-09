using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterTrigger : MonoBehaviour
{
    public event Action OnCharacterIsPresent;

    public event Action OnCharacterIsNotPresent;

    private HybridCharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HybridCharacterController GetCharacterController()
    {
        return _characterController;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            _characterController = other.GetComponent<HybridCharacterController>();
            OnCharacterIsPresent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HybridCharacterController>())
        {
            _characterController = null;
            OnCharacterIsNotPresent?.Invoke();
        }
    }
}
