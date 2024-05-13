using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KekBehavior : MonoBehaviour
{
    public Vector3 rootPos;
    public TextMeshPro dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        rootPos = transform.position;

    }

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
            dialogueText.text = "Ooh~ spinny~!";
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
