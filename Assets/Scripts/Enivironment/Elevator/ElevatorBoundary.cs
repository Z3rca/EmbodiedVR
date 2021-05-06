using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorBoundary : MonoBehaviour
{
   public UnityEvent ReachedEnd;
   private Elevator elevator;

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject == elevator.gameObject)
      {
         ReachedEnd.Invoke();
      }
   }
}
