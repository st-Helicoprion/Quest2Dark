using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public List<AudioClip> BGMAudioClips = new();
    public List<AudioClip> UISFXAudioClips = new();
    public List<AudioClip> FingerMonsterAudioClips = new();
    public List<AudioClip> StickGiantAudioClips = new();
    public List<AudioClip> ToysSFX = new();
 
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CheckBGMToPlay()
    {
        StartCoroutine(VolumeFadeOut());

        audioSource.clip = BGMAudioClips[BGMSwitcherReporter.currentAreaID];

        StartCoroutine(VolumeFadeIn());
    }

    public IEnumerator VolumeFadeOut()
    {
        while(audioSource.volume>0)
        {
            audioSource.volume-=0.075f;
            yield return null;
        }
    }

    public IEnumerator VolumeFadeIn()
    {
        while (audioSource.volume <1)
        {
            audioSource.volume+=0.075f;
            yield return null;
        }

    }

    public void WeakStateMusic()
    {
        StartCoroutine(VolumeFadeOut());

        audioSource.clip = BGMAudioClips[7];
       
        StartCoroutine(VolumeFadeIn());
    }

    public void KekThemeMusic()
    {
        StartCoroutine(VolumeFadeOut());

        BGMSwitcherReporter.currentAreaID = 0;
        audioSource.clip = BGMAudioClips[8];
      
        StartCoroutine(VolumeFadeIn());
    }

    public void EndMapMusic()
    {
        StartCoroutine(VolumeFadeOut());

        audioSource.clip = BGMAudioClips[6];
       
        StartCoroutine(VolumeFadeIn());
    }

    public void GoodEndMusic()
    {
        StartCoroutine(VolumeFadeOut());

        audioSource.clip = BGMAudioClips[5];
        
        StartCoroutine(VolumeFadeIn());
    }

    public void HeartMiniStartMusic()
    {
        StartCoroutine(VolumeFadeOut());

        audioSource.clip = BGMAudioClips[9];
      
        StartCoroutine(VolumeFadeIn());
    }

    public void HeartMiniLoopMusic()
    {

        audioSource.clip = BGMAudioClips[10];
        
       
    }
}
