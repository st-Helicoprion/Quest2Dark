using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class PlayerMoveFeedback : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip moveSound;
    public ContinuousMoveProviderBase playerMovement;
    public CharacterController playerCC;
    public bool isDead;

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
                if (playerMovement.moveSpeed < 4)
                {
                    playerMovement.moveSpeed += Time.deltaTime;
                }
                else playerMovement.moveSpeed = 4;

                if (!audioSource.isPlaying)
                {
                    audioSource.volume = 0.5f;
                    audioSource.pitch = Random.Range(1, 1.3f);
                    audioSource.PlayOneShot(moveSound);
                }
            }
            else
            {
                playerMovement.moveSpeed = 2;
                audioSource.Stop();
            }

    }
}
