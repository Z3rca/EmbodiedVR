using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRespawner : MonoBehaviour
{

    public GameObject tutManagerReference;
    public GameObject floor;
    public Vector3 originalPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == floor && tutManagerReference.GetComponent<TutorialManager>().success==false)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = originalPos;
    }
}
