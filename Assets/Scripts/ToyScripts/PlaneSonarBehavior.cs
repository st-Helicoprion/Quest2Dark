using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSonarBehavior : MonoBehaviour
{
    public Material groundedMat;
    public Rigidbody rb;
    public Animator anim;
    public float existTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("END"))
        {
            Destroy(this.gameObject);
        }

        existTime -= Time.deltaTime;

        if(existTime<0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground")||other.CompareTag("Finger"))
        {
            rb.isKinematic = true;
            GetComponent<Renderer>().material= groundedMat;
            anim.Play("PlaneSonar");

        }

    }

}
