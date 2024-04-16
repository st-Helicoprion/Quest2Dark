using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrizeBoxManager : MonoBehaviour
{
    public static bool taken;
    public Animator anim;
    public AudioSource audioSource;
    public GameObject[] logos;

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("ShatterBox"))
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.9f&&!anim.IsInTransition(0))
            {
                Destroy(this.gameObject);
            }
            
        }
        else return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HandAnimation hand))
        {
            anim.SetTrigger("Shatter");
            audioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[2]);
            taken= true;

            if (logos.Length > 0)
            {
                for (int i = 0; i < logos.Length; i++)
                {
                    logos[i].SetActive(false);
                }
            }
            else return;
            
        }
        else return;
    }
}