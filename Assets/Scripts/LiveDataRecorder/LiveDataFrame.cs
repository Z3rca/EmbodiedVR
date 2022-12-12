using System;
using UnityEngine;


    [Serializable] public class LiveDataFrame
    {
        public double TimeStamp;
        //HMD - Camera Position
        public Vector3 NoseVector;      //HMD forward
        public Vector3 HMDPositionGlobal;
        public Quaternion HMDRotationGlobal;
        public Vector3 HMDPositionLocal;
        public Quaternion HMDRotationLocal;
        
        
        //Input
        public Vector2 MovementInput;
        public Vector3 RotationInput;
        //Left Hand representation- Global Position -> Hand position of Puppet
        public Vector3 LeftHandLocalPositon;
        public Quaternion LeftHandLocalRotation;
        public Vector3 LeftHandGlobalPosition;
        public Quaternion LeftHandGlobalRotation;
        //Right Hand representation -> Hand position of Puppet
        public Vector3 RightHandLocalPositon;
        public Quaternion RightHandLocalRotation;
        public Vector3 RightGlobalPosition;
        public Quaternion RightGlobalRotation;
        
        //Controller Representations
        public bool ControllersAreShown;
  
        //Character -  CharacterController Capsule
        public Vector3 AdjustedCharacterPosition;
        public Vector3 CharacterControllerPosition;
        public Quaternion CharacterControllerRotation;
        public bool isThirdPerson;
        
        //Puppet - Avatar related
        public Vector3 PuppetPosition;
        public Quaternion PuppetRotation;
        public Vector3 PuppetPuppetHeadPositon;
        public Quaternion PuppetPuppetHeadRotation;
       
      
        public Vector3[] LimbPositions;
        public Quaternion[] LimbRotations;
        
        //Objects in Hands
        public bool ObjectAttachedToLeftHand;
        public bool ObjectAttachedToRightHand;
        public string ObjectNameLeft;
        public string ObjectNameRight;
        
        //validity
        public ulong leftGazeValidityBitmask;   //
        public ulong rightGazeValidityBitmask; //
        public ulong combinedGazeValidityBitmask;//

        //Eyetracking
        public float eyeOpennessLeft; //
        public float eyeOpennessRight; //
        public float pupilDiameterMillimetersLeft; //
        public float pupilDiameterMillimetersRight; //
        
        //local direction
        public Vector3 eyeDirectionLeftLocal;
        public Vector3 eyeDirectionRightLocal;
        public Vector3 eyeDirectionCombinedLocal;
        
        //local position
        public Vector3 eyePositionLeftLocal;
        public Vector3 eyePositionRightLocal;
        public Vector3 eyePositionCombinedLocal;
        
        //global direction
        public Vector3 eyeDirectionLeftWorld;
        public Vector3 eyeDirectionRightWorld;
        public Vector3 eyeDirectionCombinedWorld;
        
        //global position;
        public Vector3 eyePositionLeftWorld;
        public Vector3 eyePositionCombinedWorld;
        public Vector3 eyePositionRightWorld;
        
        //hit data
        public bool HitSomething;
        public Vector3 HitPosition1;
        public Vector3 HitPosition2;
        public string HitObject1;
        public string HitObject2;
        public Vector3 HitObjectPosition1;
        public Vector3 HitObjectPosition2;

    }