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
    private Material tmp_Material;

    public Material stencilMaterial;

    private int _wallLayerNumber;
    private int _defaultLayerNumber;
    private int _playeLayerNumber;

    private bool ignoreMask;

    private Dictionary<string, Material> MaterialDictionary;
    
    // Start is called before the first frame update
    void Start()
    {
        float tmp = Mathf.Log(WallLayer, 2);
        _wallLayerNumber = (int) tmp;
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
                if (!MaterialDictionary.ContainsKey(other.gameObject.GetComponent<Renderer>().material.name))
                {
                    MaterialDictionary.Add(other.gameObject.GetComponent<Renderer>().material.name,other.gameObject.GetComponent<Renderer>().material);
                    Debug.Log("added material of " + other.name);
                    tmp_Material = other.gameObject.GetComponent<Renderer>().material;
                }
            }
            else
            {
                return;
            }
            
            stencilMaterial.mainTexture = tmp_Material.mainTexture;
            other.GetComponent<Renderer>().receiveShadows = false;
            other.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            other.gameObject.GetComponent<Renderer>().material = stencilMaterial;
        }
        
        
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (ignoreMask)
            return;
        
        stencilMaskObject.SetActive(false);
        
        if (other.gameObject.layer == _wallLayerNumber&& other.GetComponent<Renderer>()!=null)
        {
            if (MaterialDictionary.ContainsKey(other.name))
            {
                other.gameObject.GetComponent<Renderer>().material = MaterialDictionary[other.name];
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
}
