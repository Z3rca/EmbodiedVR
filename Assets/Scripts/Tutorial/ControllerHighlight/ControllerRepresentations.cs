using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Hand = Valve.VR.InteractionSystem.Hand;


public class ControllerRepresentations : MonoBehaviour
{
    public GameObject LeftController;
    public GameObject LeftPerspectiveChangeButton;
    public GameObject LeftStick;
    public GameObject LeftSqueeze;
    public GameObject TriggerButtonLeft;

    public GameObject RightController;
    public GameObject RightPerspectiveChangeButton;
    public GameObject RightStick;
    public GameObject RightSqueeze;
    
    
    public Material HighLightMaterial;

    private Material standardMaterial;
    private bool highLightButton;
    
    [SerializeField] private RemoteVR RemoteVR;

    private bool Initalized;


    private Hand _leftHand;

    private Hand _rightHand;
    // Start is called before the first frame update
    
    // Update is called once per frame
    public bool forceStop;
    private Dictionary<GameObject, bool> highlightedButtons;

    private bool _switchShowHands;
    public void ShowController(bool state)
    {
        LeftController.SetActive(state);
        RightController.SetActive(state);
    }

    private void Start()
    {
        _switchShowHands = true;
        _leftHand = Player.instance.hands[0];
        _rightHand = Player.instance.hands[1];
    }
    private void Awake()
    {
        
    }

    private void Update()
    {
        _leftHand.SetVisibility(_switchShowHands);
        _rightHand.SetVisibility(_switchShowHands);
    }

    public void ShowHands(bool state)
    {
        _switchShowHands = state;
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
        highlightedButtons.Add(TriggerButtonLeft,false);
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

    public void HighlightTriggerButton(bool state)
    {
        if (!Initalized)
        {
            initializeButtons();
            Initalized = true;
        }
        
        HighLightButton(TriggerButtonLeft,state);
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
        
        
        while (highlightedButtons[Button]||!forceStop)
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

            if (forceStop)
            {
                renderer.material.color = standardMaterial.color;
            }
        }
        Debug.Log("we are now done with highlighting");
       



    }

    public void ForceStop()
    {
        forceStop = true;
    }

    private void OnDisable()
    {
        highLightButton = false;
    }
}
