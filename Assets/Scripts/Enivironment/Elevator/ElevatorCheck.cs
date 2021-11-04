using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCheck : MonoBehaviour
{
    [SerializeField] private bool _isUpstairs;
    private bool _isInside;
    
    private GameObject avatar;
    public bool IsAvatarIsInside()
    {
        return _isInside;
    }

    public bool isUpstairs()
    {
        return _isUpstairs;
    }

    public Vector3 AvatarPosition()
    {
        return avatar.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            _isInside = true;
            avatar = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            _isInside = false;
        }
    }
}
