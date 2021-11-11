using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PuppetController : MonoBehaviour
{

    private VRIK vriK;

    private CCAnimator ccAnimator;

    [SerializeField] private GameObject avatar;

    [SerializeField]private Animator _animator;
    

    private float _maximumSpeed;
    private float _currentSpeed;
    

    private Vector3 _currentDirection;

    private bool _forcedAnimation;

    private bool _isEmbodiedCondition;

    private float _AnimationRatio;

    private Transform NeckTransform;
    private Transform ChestTransform;
    
    private Transform LeftElbowTransform;
    private Transform RightElbowTransform;
    
    private Transform LeftShoulderTransform;
    private Transform RightShoulderTransform;

    private Transform LeftPelvisTramsform;
    private Transform RightPelvisTransform;

    private Transform LeftKneeTransform;
    private Transform RightKneeTransform;

    private Transform LeftFootTransform;
    private Transform RightFootTransform;

    

    private Vector3[] limbPositions;
    private Quaternion[] limbRotations;

    private bool initialized;
    
    

    private void Start()
    {
        
        ccAnimator = GetComponent<CCAnimator>();
        ccAnimator.SetAnimator(_animator);
        vriK = GetComponent<VRIK>();

        _maximumSpeed = ccAnimator.maximumForward;

        limbPositions = new Vector3[12];
        limbRotations = new Quaternion[12];

        ExperimentManager.Instance.OnScaleCalibrationFinished += IsEmobdiedCondition;

    }

    
    
    public void IsEmobdiedCondition()
    {

        _isEmbodiedCondition = ExperimentManager.Instance.SelectedAvatar.GetComponent<HybridController>()
            .IsEmbodiedCondition();
        
        if (_isEmbodiedCondition)
        {
            LeftShoulderTransform = vriK.references.leftUpperArm;
            RightShoulderTransform = vriK.references.rightShoulder;

            LeftElbowTransform = vriK.references.leftForearm;
            RightElbowTransform = vriK.references.rightForearm;

            ChestTransform = vriK.references.chest;
            NeckTransform = vriK.references.neck;

            LeftPelvisTramsform = vriK.references.leftThigh;
            RightPelvisTransform = vriK.references.rightThigh;

            LeftKneeTransform = vriK.references.leftCalf;
            RightKneeTransform = vriK.references.rightCalf;

            LeftFootTransform = vriK.references.leftFoot;
            RightFootTransform = vriK.references.rightFoot;
            initialized = true;
        }
    }


    private void StartExperiment()
    {
        
    }
    
    private void FixedUpdate()
    {
        if (!initialized)
            return;
        limbPositions[0] = NeckTransform.position;
        limbRotations[0] = NeckTransform.rotation;

        limbPositions[1] = ChestTransform.position;
        limbRotations[1] = ChestTransform.rotation;

        limbPositions[2] = LeftShoulderTransform.position;
        limbRotations[2] = LeftShoulderTransform.rotation;

        limbPositions[3] = RightShoulderTransform.position;
        limbRotations[3] = RightShoulderTransform.rotation;

        limbPositions[4] = LeftElbowTransform.position;
        limbRotations[4] = LeftElbowTransform.rotation;

        limbPositions[5] = RightElbowTransform.position;
        limbRotations[5] = RightElbowTransform.rotation;

        limbPositions[6] = LeftPelvisTramsform.position;
        limbRotations[6] = LeftPelvisTramsform.rotation;

        limbPositions[7] = RightPelvisTransform.position;
        limbRotations[7] = RightPelvisTransform.rotation;

        limbPositions[8] = LeftKneeTransform.position;
        limbRotations[8] = LeftKneeTransform.rotation;

        limbPositions[9] = RightKneeTransform.position;
        limbRotations[9] = RightKneeTransform.rotation;
        
        limbPositions[10] = LeftFootTransform.position;
        limbRotations[10] = LeftFootTransform.rotation;
        
        limbPositions[11] = RightFootTransform.position;
        limbRotations[11] = RightFootTransform.rotation;
    }

    public Vector3[] GetLimbPositions()
    {
        return limbPositions;
    }

    public Quaternion[] GetLimbRotations()
    {
        return limbRotations;
    }

    private void Update()
    {
        if (!_isEmbodiedCondition)
        {
            return;
        }
        
        avatar.transform.localPosition = Vector3.zero;

        if (_currentSpeed < 0.01f)
        {
            //  ccAnimator.ApplyAnimation(Vector3.zero, 0f);
        }
        else
        {
            avatar.transform.localEulerAngles = Vector3.zero;
            //ccAnimator.ApplyAnimation(_currentDirection, _currentSpeed);
            
        }
        //locomotion Effect for standing and readjustment.
        if (!_forcedAnimation)
        {
//            Debug.Log(_currentSpeed + " " +_maximumSpeed);
            _AnimationRatio =  (_currentSpeed / _maximumSpeed);
            

        }
        else
        {
            _AnimationRatio = 1;
            
            
        }
        
        ccAnimator.ApplyAnimation(_currentDirection, _currentSpeed);

    }
    

    private void LateUpdate()
    {
        vriK.solver.locomotion.weight =1- _AnimationRatio;    
    }


    public void SetForcedAnimationForSeconds(float time)
    {
        if (_forcedAnimation)
            return;
        StartCoroutine(ForcedAnimationCooldown(time));
    }

    private IEnumerator ForcedAnimationCooldown(float time)
    {
        _forcedAnimation = true;
        yield return new WaitForSeconds(time);
        _forcedAnimation = false;
    }


    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }


    public void RotateAvatar(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    public void SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
    }
    public void MovePuppet(Vector3 direction)
    {
        _currentDirection = direction;
    }
    
    
    
    
    

}
