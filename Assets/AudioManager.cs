using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnOffMusic(bool On)
    {
        if(On)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
