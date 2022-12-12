using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioInstructionsController : MonoBehaviour
{
    [SerializeField] private List<AudioSource> potentialIntferingSources;
    public AudioSource audioSource;

    private bool playedAudio = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAudio()
    {
        if (potentialIntferingSources.Count>0)
        {
            bool isPlaying = false;
            foreach (var source in potentialIntferingSources.ToList())
            {
                if (source.isPlaying)
                {
                    isPlaying = true;
                }
                else
                {
                    potentialIntferingSources.Remove(source);
                }
            }
            if(isPlaying)
                StartCoroutine(WaitForClipPlayedThenPlay(potentialIntferingSources));
            else
            {
                if (!playedAudio)
                {
                    Debug.Log("audio sources were all quite");
                    audioSource.Play();
                    playedAudio = true;
                }
            }

        }
        else
        {
            if (!playedAudio)
            {
                Debug.Log("not one source was playing");
                audioSource.Play();
                playedAudio = true;
            }
        }
        
        
    }
    
    private IEnumerator WaitForClipPlayedThenPlay(List<AudioSource> audioSources)
    {
        foreach (var source in audioSources)
        {
            var done=false;
            while (!done)
            {
                if (!audioSource.isPlaying)
                {
                    done = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(2f);
        
        if (!playedAudio)
        {
            audioSource.Play();
            playedAudio = true;
        }
    }
}
