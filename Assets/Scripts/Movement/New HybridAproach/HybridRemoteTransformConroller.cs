using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HybridRemoteTransformConroller : MonoBehaviour
{
    public Player SteamVRplayer;
    public Transform LocalHMD;
    public Transform RemoteHMD;
    
    
    public Transform RemoteFeetPositionGuess;

    public Transform RemoteForwardGuess;

    public Transform LocalLeft;
    public Transform LocalRight;
    public Transform RemoteLeft;
    public Transform RemoteRight;

    private bool readjusting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 BodyRotation= SteamVRplayer.bodyDirectionGuess;
        RemoteForwardGuess.transform.localRotation = Quaternion.LookRotation(BodyRotation, Vector3.up);
        Vector3 feetpositon = SteamVRplayer.transform.InverseTransformPoint(SteamVRplayer.feetPositionGuess);
        RemoteFeetPositionGuess.transform.localPosition = feetpositon;
        
        
        RemoteHMD.transform.localPosition = LocalHMD.transform.localPosition;
        RemoteHMD.transform.localRotation = LocalHMD.transform.localRotation;
        
    }
    
    
    public void SetPosition(Vector3 position)
    {
        transform.position= position;
    }
    
    public void RotateRemoteTransforms(Quaternion rotation)
    {
        transform.rotation= rotation;
    }
    
    public Quaternion GetRemoteFowardGuess()
    {
        return RemoteForwardGuess.localRotation;
    }
    
    public Vector3 GetLocalRemoteFeetPositionGuess()
    {
        return RemoteFeetPositionGuess.transform.localPosition;
    }
}
