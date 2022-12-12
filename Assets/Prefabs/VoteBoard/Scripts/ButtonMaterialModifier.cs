using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonMaterialModifier : MonoBehaviour
{
    public GameObject ApplyGameObject;
    
    private  Material _defaultMaterial;

    public Material changeMaterial;

    public Material lockMaterial;


    private void Awake()
    {
        _defaultMaterial = ApplyGameObject.GetComponent<Renderer>().material;
    }

    private void Start()
    {
        
    }
    // Start is called before the first frame update
    public void ChangeMaterial()
    {
        if (changeMaterial != null)
        {
            ApplyGameObject.GetComponent<Renderer>().material = changeMaterial;
        }

        
    }

    public void LockMaterial()
    {
        if (lockMaterial != null)
        {
            ApplyGameObject.GetComponent<Renderer>().material = lockMaterial;
        }
    }

    public void RestoreMaterial()
    {
        ApplyGameObject.GetComponent<Renderer>().material = _defaultMaterial;
    }
    
}
