using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class SummonsDoorComp : MonoBehaviour
{
    public Animator anim;
    public static bool open;

    // Update is called once per frame
    void Update()
    {
        if(open)
        {
            anim.SetTrigger("Open");
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && !anim.IsInTransition(0))
            {
                open = false;
                Destroy(this.gameObject);
            }
        }
    }
}
