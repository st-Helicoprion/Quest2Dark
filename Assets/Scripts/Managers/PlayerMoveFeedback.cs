using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class PlayerMoveFeedback : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip moveSound, runSound;
    public ContinuousMoveProviderBase playerMovement;
    public CharacterController playerCC;
    public bool isDead;
    public float runSoundCount;

    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {

            if (Mathf.Abs(playerCC.velocity.z) > 0.5f || Mathf.Abs(playerCC.velocity.x) > 0.5f)
            {
                if (playerMovement.moveSpeed < 4.5f)
                {
                    playerMovement.moveSpeed += Time.deltaTime;
                }
                else playerMovement.moveSpeed = 4.5f;

                if (!audioSource.isPlaying&&playerMovement.moveSpeed<3)
                {
                    audioSource.volume = 0.5f;
                    audioSource.pitch = Random.Range(1, 1.3f);
                    audioSource.PlayOneShot(moveSound);
                }
                if (!audioSource.isPlaying&&playerMovement.moveSpeed > 3)
                {
                    audioSource.volume = 0.5f;
                    //audioSource.pitch = Random.Range(1, 1.3f);
                    audioSource.PlayOneShot(runSound);
                    
                }

            }
            else
            {
                playerMovement.moveSpeed = 1.5f;
                audioSource.Stop();
            }

    }
}
