using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KekBehavior : MonoBehaviour
{
    public TextMeshPro dialogueText;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "woodairplane")
        {
            this.tag = "Untagged";
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("TopSonar"))
        {
            if (DialogueManager.instance.language == DialogueManager.LangSelect.EN)
            {
                dialogueText.text = "Ooh~ spinny~!";

            }
            else if (DialogueManager.instance.language == DialogueManager.LangSelect.ZH)
            {
                dialogueText.text = "ÂàÂàÂà~!";
            }
            GameObject top = GameObject.FindWithTag("SpinningTop");
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TopSonar"))
        {
            dialogueText.text = "";

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            
            dialogueText.text = "*#@<Y*9!";
            StartCoroutine(KnockbackCoroutine());

        }
    }

    IEnumerator KnockbackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueText.text = "";
    }

}
