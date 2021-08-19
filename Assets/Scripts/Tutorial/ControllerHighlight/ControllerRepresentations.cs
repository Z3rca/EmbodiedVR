using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerRepresentations : MonoBehaviour
{
    public GameObject LeftController;
    public GameObject LeftPerspectiveChangeButton;
    public GameObject LeftStick;
    public GameObject LeftSqueeze;
    

    public GameObject RightController;
    public GameObject RightPerspectiveChangeButton;
    public GameObject RightStick;
    public GameObject RightSqueeze;
    
    public Material HighLightMaterial;

    private Material standardMaterial;
    private bool highLightButton;
    
    [SerializeField] private RemoteVR RemoteVR;

    private bool Initalized;
    // Start is called before the first frame update
    
    // Update is called once per frame

    private Dictionary<GameObject, bool> highlightedButtons;
    public void ShowController(bool state)
    {
        LeftController.SetActive(state);
        RightController.SetActive(state);
    }

    private void Awake()
    {
        
    }

    private void initializeButtons()
    {
        highlightedButtons = new Dictionary<GameObject, bool>();
        highlightedButtons.Add(LeftPerspectiveChangeButton, false);
        highlightedButtons.Add(LeftStick, false);
        highlightedButtons.Add(LeftSqueeze, false);
        highlightedButtons.Add(RightPerspectiveChangeButton, false);
        highlightedButtons.Add(RightStick, false);
        highlightedButtons.Add(RightSqueeze, false);
    }
    public void HighLightPerspectiveChangeButtons(bool state)
    {
        if (!Initalized)
        {
            initializeButtons();
            Initalized = true;
        }
        HighLightButton(LeftPerspectiveChangeButton, state);
        HighLightButton(RightPerspectiveChangeButton, state);
    }

    public void HighlightMovementStick(bool state)
    {
        if (!Initalized)
        {
            initializeButtons();
            Initalized = true;
        }
        
        HighLightButton(LeftStick,state);
    }
    
    public void HighlightRotationStick(bool state)
    {
        if (!Initalized)
        {
            initializeButtons();
            Initalized = true;
        }
        
        HighLightButton(RightStick,state);
    }

    public void HighLightGraspButtons(bool state)
    {
        if (!Initalized)
        {
            initializeButtons();
            Initalized = true;
        }
        HighLightButton(LeftSqueeze, state);
        HighLightButton(RightSqueeze, state);
    }
    
    private void HighLightButton(GameObject TargetButton, bool state)
    {
        if (highlightedButtons.ContainsKey(TargetButton))
        {
            Debug.Log("found controllers" + state);
            if (state)
            {
                highlightedButtons[TargetButton] = true;
                StartCoroutine(HighLightButtonCoroutine(TargetButton));
            }
            else
            {
                if(highlightedButtons[TargetButton])
                    highlightedButtons[TargetButton] = false;
            }
        }
        else
        {
            Debug.LogWarning("Button not found");
        }
    }
    
    

    private IEnumerator HighLightButtonCoroutine(GameObject Button)
    {
        Debug.Log(Button);
        var renderer = Button.GetComponent<Renderer>();
        standardMaterial = renderer.material;

        renderer.material = HighLightMaterial;

        renderer.material.color = Color.yellow;

        bool switchDirection = true;
        
        
        while (highlightedButtons[Button])
        {
//            Debug.Log("running " + renderer.material.color.r + " "  +renderer.material.color.g);
            if (switchDirection)
            {
                renderer.material.color -= Color.yellow * 0.01f;
            }
            else
            {
                renderer.material.color += Color.yellow * 0.01f;
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



    }
    
    

    private void OnDisable()
    {
        highLightButton = false;
    }
}
