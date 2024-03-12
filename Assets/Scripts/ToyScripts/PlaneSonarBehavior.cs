using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSonarBehavior : MonoBehaviour
{
    public Animator anim;
    public float existTime;
    // Start is called before the first frame update
    void Start()
    {
       
        anim = GetComponent<Animator>();
        anim.Play("PlaneSonar");
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

  
}
