using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSonarReporter : MonoBehaviour
{
    public Transform playerSonar;
    public bool primed;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerSonar = GameObject.Find("PlayerSonar").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerSonar!= null)
        {
            if(playerSonar.position.y>transform.position.y)
            {
                audioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[3]);
                TutorialsManager.instance.cicadaGoal = true;
                Destroy(transform.parent.gameObject);
            }
        }

        if(primed)
        {
            if(CustomTopManager.isSpinning)
            {
                audioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[3]);
                TutorialsManager.instance.topGoal = true;
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("SpinningTop"))
        {
            primed = true;
        }
    }
}
