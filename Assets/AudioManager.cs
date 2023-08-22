using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public bool isPlaying = true;
    public bool isSoundEffectOn = true;
    // Start is called before the first frame update
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnOffMusic(bool On)
    {
        if(On)
        {
            isPlaying = true;
            audioSource.Play();
        }
        else
        {
            isPlaying = false;
            audioSource.Stop();
        }
    }
    public void PlaySoundEffect(AudioClip clip)
    {
        if(!isSoundEffectOn) return;
        audioSource.PlayOneShot(clip);
    }
}
