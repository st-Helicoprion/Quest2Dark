using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        CheckBGMToPlay();
    }

    public void CheckBGMToPlay()
    {
        Debug.Log("audioclip confirmed");
        //if (SceneManager.GetActiveScene().name == "RogueRoomScene")
        if (SceneManager.GetActiveScene().name == "LabyrinthGameScene"||SceneManager.GetActiveScene().name=="MechanicsTest")
        {
            /*audioSource.clip = BGMAudioClips[0];
            audioSource.Play();*/
        }
        /*else if(SceneManager.GetActiveScene().name == "GameLevelMain")
        {
            audioSource.clip = BGMAudioClips[1];
            audioSource.Play();
        }*/
    }

    


}
