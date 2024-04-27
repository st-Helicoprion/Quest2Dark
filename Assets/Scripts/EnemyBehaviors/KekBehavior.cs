using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KekBehavior : MonoBehaviour
{
    public Vector3 rootPos;
    public bool offRoot, attracted;
    public TextMeshPro dialogueText;
    public UIViewAligner alignment;

    // Start is called before the first frame update
    void Start()
    {
        rootPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != rootPos&&!attracted)
        { 
            offRoot= true;
        }
        else
        {
            offRoot= false;
        }

        if(offRoot&&!attracted)
        {
            ReturnToRootPos();
        }
    }

 /*   private void OnTriggerStay(Collider other)
    {
        
        if(other.CompareTag("TopSonar"))
        {
            attracted = true;
            dialogueText.text = "Ooh~ spinny~!";
            Vector3 direction = GameObject.FindWithTag("SpinningTop").transform.position - transform.position;
            transform.position += 0.1f * direction;
            transform.LookAt(direction);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TopSonar"))
        {
            attracted = false;
            dialogueText.text = "";
            rootPos = transform.position;
            
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            dialogueText.text = "*#@<Y*9!";
            StartCoroutine(KnockbackCoroutine(collision));

        }
    }
    void ReturnToRootPos()
    {
        
        Vector3 direction = rootPos - transform.position;
        transform.position += 0.1f*direction;
        transform.LookAt(direction);
    }

    IEnumerator KnockbackCoroutine(Collision collision)
    {
        for (int i = 0; i < 2; i++)
        {
            alignment.enabled = false;
            transform.localPosition -= (collision.transform.position - transform.position);
            yield return new WaitForSeconds(0.5f);
        }
        alignment.enabled = true;
        dialogueText.text = "";
    }

}
