using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableExamples : MonoBehaviour
{

    public GameObject examples;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Activate DebugLineRenderers
        if (Input.GetKeyDown(KeyCode.E))
        {
            examples.SetActive(!examples.activeSelf);
        }
    }
}
