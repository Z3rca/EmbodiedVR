using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridCameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 targetPosition;


    private void Start()
    {
        speed = 1;
        targetPosition = this.transform.position;
    }

    public void RotateCamera(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    public void SetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * speed);
    }
}
