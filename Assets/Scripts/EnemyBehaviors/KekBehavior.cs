using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KekBehavior : MonoBehaviour
{
    public Vector3 rootPos;
    public bool offRoot, attracted;
    public TextMeshPro dialogueText;

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

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("TopSonar"))
        {
            attracted = true;
            dialogueText.text = "Ooh~ spinny~!";
            GameObject top = GameObject.FindWithTag("SpinningTop");
            Vector3 direction = new Vector3(top.transform.position.x - transform.position.x, transform.position.y, top.transform.position.z - transform.position.z);
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

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            dialogueText.text = "*#@<Y*9!";
            StartCoroutine(KnockbackCoroutine(collision));

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name=="woodairplane")
        {
            transform.tag = "Untagged";
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
        for (int i = 0; i < 5; i++)
        {
            transform.position -= Vector3.forward;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = "";
    }

}
