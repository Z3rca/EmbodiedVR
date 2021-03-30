using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject RotationAxis;
    public GameObject Camera;
    public GameObject Player;
    public GameObject ThirdPersonPosition;
    public GameObject CameraArm;

    public bool VrObjectOriented;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VrObjectOriented)
        {
            Vector3 currentRotation = RotationAxis.transform.rotation.eulerAngles;
            Vector3 eulerRotAxis = Camera.transform.rotation.eulerAngles;
            eulerRotAxis.x = 0;
            eulerRotAxis.z = 0;

           //eulerRotAxis.y= Mathf.Lerp(currentRotation.y, eulerRotAxis.y, Time.deltaTime * 2f);
            
            RotationAxis.transform.rotation = Quaternion.Lerp(RotationAxis.transform.rotation, Quaternion.Euler(eulerRotAxis),Time.deltaTime*2f );
            
            Player.transform.position = CameraArm.transform.position;
        }
        
    }
}
