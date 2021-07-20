using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerRepresentations : MonoBehaviour
{
    public GameObject LeftController;
    public GameObject LeftPerspectiveChangeButton;
    public GameObject LeftSqueeze;
    

    public GameObject RightController;
    public GameObject RightPerspectiveChangeButton;
    public GameObject RightSqueeze;
    
    
    public Material HighLightMaterial;

    private Material standardMaterial;
    private bool highLightButton;
    
    [SerializeField] private RemoteVR RemoteVR;
    // Start is called before the first frame update
    
    // Update is called once per frame
    public void ShowController(bool state)
    {
        LeftController.SetActive(state);
        RightController.SetActive(state);
    }

    public void HighLightPerspectiveChangeButton(bool state)
    {
        if (state)
        {
            if (highLightButton)
            {
                return;
            }
            else
            {
                StartCoroutine(HighLightButton());
            }
        }
        else
        {
            if(highLightButton)
                highLightButton = false;
        }
        
        
        
        
    }

    private IEnumerator HighLightButton()
    {
        
        highLightButton = true;
        var renderer = LeftPerspectiveChangeButton.GetComponent<Renderer>();
        var renderer2 = RightPerspectiveChangeButton.GetComponent<Renderer>();
        standardMaterial = renderer.material;

        renderer.material = HighLightMaterial;
        renderer2.material = HighLightMaterial;
        
        renderer.material.color = Color.yellow;
        renderer2.material.color = Color.yellow;

        bool switchDirection = true;

        while (highLightButton)
        {
            Debug.Log("running " + renderer.material.color.r + " "  +renderer.material.color.g);
            if (switchDirection)
            {
                renderer.material.color -= Color.yellow * 0.01f;
                renderer2.material.color -= Color.yellow * 0.01f;
            }
            else
            {
                renderer.material.color += Color.yellow * 0.01f;
                renderer2.material.color += Color.yellow * 0.01f;
            }
            

            if (renderer.material.color.r>=1f&& renderer.material.color.g>=1f)
            {
                switchDirection = true;
            }

            if (renderer.material.color.r<=0f&& renderer.material.color.g<=0f)
            {
                switchDirection = false;
            }

            yield return new WaitForSeconds(0.01f);
        }

        renderer.material = standardMaterial;
        renderer2.material = standardMaterial;



    }
    


    private void Update()
    {


        LeftController.transform.localPosition=RemoteVR.LocalLeft.transform.localPosition;
        LeftController.transform.localRotation=RemoteVR.LocalLeft.transform.localRotation;
        
        RightController.transform.localPosition=RemoteVR.LocalRight.transform.localPosition;
        RightController.transform.localRotation=RemoteVR.LocalRight.transform.localRotation;
    }

    private void OnDisable()
    {
        highLightButton = false;
    }
}
