using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalBox : MonoBehaviour
{

    public int counter = 0;

    public TMP_Text visibleCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Sphere")
        {
            counter++;
            visibleCounter.text = counter + "/10 spheres collected! \r\n" + (9-counter) + " are still needed to exit.";
        }
    }
}
