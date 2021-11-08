using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<GameObject, bool> _highlightedButtons;

    private List<GameObject> _buttons;

    private bool _switchShowHands;
    public void ShowController(bool state)
    {
        Debug.Log("show controllers: " + state);
        LeftController.SetActive(state);
        RightController.SetActive(state);
    }

    private void Start()
    {
        _switchShowHands = true;
        _leftHand = Player.instance.hands[0];
        _rightHand = Player.instance.hands[1];
        
        initializeButtons();
        
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
        _highlightedButtons = new Dictionary<GameObject, bool>();
        _buttons = new List<GameObject>();
        _highlightedButtons.Add(LeftPerspectiveChangeButton, false);
        _highlightedButtons.Add(LeftStick, false);
        _highlightedButtons.Add(LeftSqueeze, false);
        _highlightedButtons.Add(RightPerspectiveChangeButton, false);
        _highlightedButtons.Add(RightStick, false);
        _highlightedButtons.Add(RightSqueeze, false);
        _highlightedButtons.Add(TriggerButtonLeft,false);
        
        
        foreach (var key in _highlightedButtons.Keys)
        {
            _buttons.Add(key);
        }
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
        if (_highlightedButtons.ContainsKey(TargetButton))
        {
            Debug.Log("found controllers" + state);
            if (state)
            {
                _highlightedButtons[TargetButton] = true;
                StartCoroutine(HighLightButtonCoroutine(TargetButton));
            }
            else
            {
                if(_highlightedButtons[TargetButton])
                    _highlightedButtons[TargetButton] = false;
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
        
        
        while (_highlightedButtons[Button])
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
        
        
        renderer.material.color = standardMaterial.color;
        
        Debug.Log("we are now done with highlighting");
       



    }

    public void ForceStop()
    {
        foreach (var key in _buttons)
        {
            _highlightedButtons[key] = false;
        }
    }

    private void OnDisable()
    {
        highLightButton = false;
    }
}
