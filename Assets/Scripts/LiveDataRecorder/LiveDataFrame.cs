using System;
using UnityEngine;


    [Serializable] public class LiveDataFrame
    {
        //HMD - Camera Position
        public Vector3 NoseVector;      //HMD forward
        public Vector3 HMDPositionGlobal;
        public Quaternion HMDRotationGlobal;
        public Vector3 HMDPositionLocal;
        public Quaternion HMDRotationLocal;
        
        //Input
        public Vector2 MovementInput;
        //LeftController - Global Position -> Hand position of Puppet
        public Vector3 LeftHandLocalPositon;
        public Quaternion LeftHandLocalRotation;
        public Vector3 LeftHandGlobalPosition;
        public Quaternion LeftHandGlobalRotation;
        //Right Controller - Global Position -> Hand position of Puppet
        public Vector3 RightHandLocalPositon;
        public Quaternion RightHandLocalRotation;
        public Vector3 RightGlobalPosition;
        public Quaternion RightGlobalRotation;
        
        //Character -  CharacterController Capsule
        public Vector3 CharacterControllerPosition;
        public Quaternion CharacterControllerRotation;
        public bool isThirdPerson;
        
        //Puppet - Avatar related
        public Vector3 PuppetPosition;
        public Quaternion PuppetRotation;
        public Vector3 PuppetPuppetHeadPositon;
        public Quaternion PuppetPuppetHeadRotation;
       
      
        
    }