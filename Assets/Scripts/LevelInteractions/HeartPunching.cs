using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPunching : MonoBehaviour
{
    public float hitBuffer;
    public float hitRate;
    public static bool stopTime;
    public Animator anim;
    public AudioSource audioSource;

    private void Update()
    {
        hitBuffer -= Time.deltaTime;

        if(hitBuffer<0)
        {
            stopTime= true;
        }
        else stopTime= false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            hitBuffer = hitRate;
            anim.SetTrigger("Pulse");
            float rand = Random.Range(0.8f, 1.3f);
            audioSource.pitch = rand;
            audioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[4]);
           
        }
    }
}
