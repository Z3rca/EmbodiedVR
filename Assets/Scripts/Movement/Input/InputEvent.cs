using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementData: EventArgs
{
   public float speed;
   public Vector2 input;
   public Vector3 WorldPosition;
   public Quaternion RotationInWorld;
   public double timeStamp;
}
