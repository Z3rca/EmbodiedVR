using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Debug = UnityEngine.Debug;

public class StencilWallDection : MonoBehaviour
{
    public GameObject TargetForMask;
    public GameObject stencilMaskObject;
    public LayerMask WallLayer;
    public LayerMask DefaultLayer;
    public LayerMask PlayerLayer;
    private Material formerMaterial;

    public Material stencilMaterial;

    private int _wallLayerNumber;
    private int _defaultLayerNumber;
    private int _playeLayerNumber;

    private bool ignoreMask;

    private Dictionary<string, Material> MaterialDictionary;

    private List<GameObject> _changedGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        float tmp = Mathf.Log(WallLayer, 2);
        _wallLayerNumber = (int) tmp;
        _changedGameObjects = new List<GameObject>();
        MaterialDictionary = new Dictionary<string, Material>();

    }

    // Update is called once per frame
    void Update()
    {
        stencilMaskObject.transform.position = TargetForMask.transform.position;
    }


    private void OnTriggerStay(Collider other)
    {
       
       // Debug.Log(other.gameObject.layer + " " + other.name);
        if (other.gameObject.layer == _wallLayerNumber)
        {
            if (ignoreMask)
                return;
            stencilMaskObject.SetActive(true);
//            Debug.Log("wall object " + other.name);
            if (other.GetComponent<Renderer>()!=null)
            {
                Material tmpMaterial = other.gameObject.GetComponent<Renderer>().material;
                if (!MaterialDictionary.ContainsKey(other.gameObject.name))
                {
                    MaterialDictionary.Add(other.gameObject.name,other.gameObject.GetComponent<Renderer>().material);
                }
                
                stencilMaterial.mainTexture = tmpMaterial.mainTexture;
                other.GetComponent<Renderer>().receiveShadows = false;
                other.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                other.gameObject.GetComponent<Renderer>().material = stencilMaterial;

                if (!_changedGameObjects.Contains(other.gameObject))
                {
                    _changedGameObjects.Add(other.gameObject);
                }
            }
        }
        
        
        
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (ignoreMask)
            return;
        
        stencilMaskObject.SetActive(false);
        
        if (other.gameObject.layer == _wallLayerNumber&& other.GetComponent<Renderer>()!=null)
        {
            if (MaterialDictionary.ContainsKey(other.gameObject.name))
            {
                other.gameObject.GetComponent<Renderer>().material = MaterialDictionary[other.gameObject.name];
            }
            
            //other.gameObject.GetComponent<Renderer>().material = tmp_Material;
            other.GetComponent<Renderer>().receiveShadows = true;
            other.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
        }

    }


    public void IgnoreMask(bool state)
    {
        if (state==true)
        {
            stencilMaskObject.SetActive(false);
            ignoreMask=true;
        }
        else
        {
            ignoreMask = false;
        }
        
    }

    public void OnDisable()
    {
        foreach (var obj in _changedGameObjects)
        {
            obj.GetComponent<Renderer>().material = MaterialDictionary[obj.name];

          
        }
        
        MaterialDictionary.Clear();

        _changedGameObjects.Clear();
    }
}
