using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HybridRemoteTransformConroller : MonoBehaviour
{
    public Player SteamVRplayer;
    public Transform LocalHMD;
 //   public Transform LocalLeft;
   // public Transform LocalLeftArm;
  //  public Transform LocalRight;
    //public Transform LocalRightArm;
    //public Transform LocalHeadTarget;
    public Transform RemoteFeetPositionGuess;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 feetpositon = SteamVRplayer.transform.InverseTransformPoint(SteamVRplayer.feetPositionGuess);
        RemoteFeetPositionGuess.transform.localPosition = feetpositon;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position= position;
    }
    
    public void RotateRemoteTransforms(Quaternion rotation)
    {
        transform.rotation= rotation;
    }


    public Vector3 GetRemoteFeetPositionGuess()
    {
        return RemoteFeetPositionGuess.transform.position;
    }
}
