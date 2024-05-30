using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

[RequireComponent(typeof(AudioSource))]
public class PlayerMoveFeedback : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip moveSound, runSound;
    public ContinuousMoveProviderBase playerMovement;
    public CharacterController playerCC;
    public static bool isDead, moving;
    public float runSoundCount, footstepsInterval;
    public Transform[] footsteps;
    public Transform cam;
    public int stepsNum;

    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        isDead = false;
        stepsNum = 0;

        FindFootsteps();
    }

    // Update is called once per frame
    void Update()
    {

            if (Mathf.Abs(playerCC.velocity.z) > 0.5f || Mathf.Abs(playerCC.velocity.x) > 0.5f)
            {
                moving = true;
               
                footstepsInterval -= Time.deltaTime;
                WallFinderUI.blockerInterval -= Time.deltaTime;
                if (playerMovement.moveSpeed < 5)
                {
                    playerMovement.moveSpeed += Time.deltaTime;
                }
                else playerMovement.moveSpeed = 5;

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

                if(footstepsInterval<0)
                {
                    footstepsInterval = 0.2f;
                    
                    if (stepsNum<footsteps.Length)
                    {
                         MoveFootstepsToPlayer();
                    }
                    else
                    {
                        stepsNum = 0;
                        MoveFootstepsToPlayer();
                    }
                }

            }
            else
            {
                moving= false;
                playerMovement.moveSpeed = 1.5f;
                audioSource.Stop();
            }

    }

    void FindFootsteps()
    {
        Transform footstepsContainer = GameObject.Find("PlayerSteps").transform;

        for (int i =0; i<footstepsContainer.childCount; i++)
        {
            footsteps[i] = footstepsContainer.GetChild(i).transform;
        }
    }

    void MoveFootstepsToPlayer()
    {
        Vector3 rotHelper = cam.rotation.eulerAngles;
        rotHelper.x = 0; rotHelper.z = 0;
        footsteps[stepsNum].position = new Vector3(playerCC.transform.position.x, footsteps[stepsNum].position.y, playerCC.transform.position.z);
        footsteps[stepsNum].rotation = Quaternion.Euler(rotHelper);
        stepsNum++;
    }

}
