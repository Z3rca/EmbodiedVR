using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RotatonElevator : MonoBehaviour
{
    public GameObject Lift;
    public GameObject door;
    private bool running;
    public float speed;
    private bool atTarget;
    public RotationAffector _rotationAffector;
    [SerializeField] private bool clockwise;
    private PhysicalMovement physicalMovement;

    private GameObject physicalMovementPlayer;

    private bool launchedStart;

    // Start is called before the first frame update
    void Start()
    {
        _rotationAffector.speed = 0f;
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TurnPlatform();
        }
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            TurnPlatform();
        }
    }



    public void TurnPlatform()
    {
        if (!running)
        {
            running = true;
            atTarget = false;
            if (clockwise)
            {
                _rotationAffector.speed = 100;
                StartCoroutine(TurnPlatform(speed, true, new Vector3(0, 180, 0)));
            }
            else
            {
                _rotationAffector.speed = -15;
                StartCoroutine(TurnPlatform(speed, false, new Vector3(0, 0, 0)));
            }
           
        }
        
        
    }


    IEnumerator TurnPlatform(float speed, bool rightSide, Vector3 eulerRotation)
    {
        while (!atTarget)
        {
          Vector3 currentEuler=   this.transform.rotation.eulerAngles;
          if (rightSide)
          {
              if (Quaternion.Angle(this.transform.rotation,Quaternion.Euler(eulerRotation))<0.01f)
              {
                  atTarget = true;
                  break;
              }
              else
              {
                 
                  this.transform.Rotate(Vector3.up * (+speed * Time.deltaTime));
              }
          }
          else
          {
              if (Quaternion.Angle(this.transform.rotation,Quaternion.Euler(eulerRotation))<0.01f)
              {
                  atTarget = true;
                  break;
              }
              else
              {
                 
                  this.transform.Rotate(Vector3.up * (-speed * Time.deltaTime));
              }
          }
          

          yield return new WaitForFixedUpdate();
        }

        clockwise = false;
        _rotationAffector.speed = 0f;
        _rotationAffector.SetActive(false);
        door.SetActive(false);
        physicalMovementPlayer.GetComponent<PhysicalMovement>().transform.SetParent(null);
        running = false;
    }

    IEnumerator WaitForStart(GameObject other, float seconds)
    {
            yield return new WaitForSeconds(seconds);
            physicalMovement = other.GetComponent<PhysicalMovement>();
            _rotationAffector.SetPhysicalMovement(physicalMovement);
            _rotationAffector.SetActive(true);
            door.SetActive(true);
            TurnPlatform();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhysicalMovement>())
        {
            if (launchedStart)
                return;

            physicalMovementPlayer = other.gameObject;
            
            launchedStart = true;
            physicalMovementPlayer.transform.SetParent(this.transform);
            StartCoroutine(WaitForStart(physicalMovementPlayer, 5f));
        }
        
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("true that");
        if (other.GetComponent<PhysicalMovement>())
        {
            _rotationAffector.SetActive(false);
        }
        
    }
}
