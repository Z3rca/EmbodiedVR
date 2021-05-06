using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR.InteractionSystem;
using Debug = UnityEngine.Debug;

public class StencilWallDection : MonoBehaviour
{
    public LayerMask WallLayer;
    public LayerMask DefaultLayer;
    public LayerMask PlayerLayer;
    private Material formerMaterial;
    private Material tmp_Material;

    public Material stencilMaterial;

    private int _wallLayerNumber;
    private int _defaultLayerNumber;
    private int _playeLayerNumber;
    // Start is called before the first frame update
    void Start()
    {
        float tmp = Mathf.Log(WallLayer, 2);
        _wallLayerNumber = (int) tmp;
        
      Debug.Log(" layer number is "+ _wallLayerNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer + " " + other.name);
        if (other.gameObject.layer == _wallLayerNumber)
        {
            Debug.Log("wall object " + other.name);
            if (other.GetComponent<Renderer>()!=null)
            {
                tmp_Material = other.gameObject.GetComponent<Renderer>().material;
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
        if (other.gameObject.layer == _wallLayerNumber&& other.GetComponent<Renderer>()!=null)
        {
            other.gameObject.GetComponent<Renderer>().material = tmp_Material;
            other.GetComponent<Renderer>().receiveShadows = true;
            other.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
        }

    }
}
