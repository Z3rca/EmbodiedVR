using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeRespawner : MonoBehaviour
{

    public GameObject tutManagerReference;
    public GameObject floor;
    public GameObject originalPos;

    public UnityEvent RespawnedCube;
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
        if (other.gameObject == floor && !tutManagerReference.GetComponent<TutorialManager>().GetIsTutorialFinished())
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = originalPos.transform.position;
        RespawnedCube.Invoke();
    }
}
