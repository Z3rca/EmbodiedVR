using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TextSelectionController : MonoBehaviour
{
    [SerializeField] private string germanText;
    [SerializeField] private string englishText;

    private TextMeshProUGUI _textField;
    // Start is called before the first frame update
    
    void Start()
    {
        _textField = GetComponent<TextMeshProUGUI>();
        ExperimentManager.Instance.GetComponent<AudioSelectionManager>().RegisterTextController(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGermanText()
    {
        _textField.text = germanText;
    }
    public void SetEnglishText()
    {
        _textField.text = englishText;
    }
}
