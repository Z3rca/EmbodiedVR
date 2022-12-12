using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialInteractionTrigger : MonoBehaviour
{
    public bool goalBox = false;
    private bool isInsideTrigger;
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.GetComponent<HybridCharacterController>())
        {
            if (TutorialManager.Instance.GetIsTutorialFinished())
                return;

            
            if(!goalBox)
                if (TutorialManager.Instance.CubeIsInHand())
                    return;
            
            
            
            
            isInsideTrigger = true;

            if (goalBox)
            {
                Debug.Log("is in second area");
                if(TutorialManager.Instance.CubeIsInHand())
                    TutorialManager.Instance.ReachedSecondInteractionArea();
            }
            else
            {
                TutorialManager.Instance.ReachedInteractionArea();
            }
            
        }
        
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (goalBox)
        {
            if (TutorialManager.Instance.ReachedSecondArea())
            {
                TutorialManager.Instance.ReachedSecondArea(false);
            }
        }
        
        StartCoroutine(DelayUntilOff(2f));
    }

    private IEnumerator DelayUntilOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isInsideTrigger = false;
    }*/
}
