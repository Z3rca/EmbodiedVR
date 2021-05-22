using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    public static TutorialManager Instance { get; private set; }
    private TutorialAudioDialogController audioController;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        audioController = GetComponent<TutorialAudioDialogController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartTutorial()
    {
        StartCoroutine(FamilarizationRoutine());
    }


    private IEnumerator FamilarizationRoutine()
    {
        audioController.FamilarizationAudioClip();
        yield return new WaitUntil(() => !audioController.GetPlayingAudioStatus());
        Debug.Log("finished");
    }
}
