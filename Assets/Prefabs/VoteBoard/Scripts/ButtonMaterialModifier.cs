using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaterialModifier : MonoBehaviour
{
    public GameObject ApplyGameObject;
    
    private  Material _defaultMaterial;

    public Material changeMaterial;

    private void Start()
    {
        _defaultMaterial = ApplyGameObject.GetComponent<Renderer>().material;
    }
    // Start is called before the first frame update
    public void ChangeMaterial()
    {
        if (changeMaterial != null)
        {
            ApplyGameObject.GetComponent<Renderer>().material = changeMaterial;
        }
    }

    public void RestoreMaterial()
    {
        ApplyGameObject.GetComponent<Renderer>().material = _defaultMaterial;
    }
    
}
