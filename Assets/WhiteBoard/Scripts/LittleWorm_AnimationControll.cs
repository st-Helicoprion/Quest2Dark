using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleWorm_AnimationContorll : MonoBehaviour
{
    Animator animator;
    public string ColliderName;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == ColliderName)
        {
            Debug.Log("enter");
            animator.SetBool("Enter?", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == ColliderName)
        {
            Debug.Log("exit");
            animator.SetBool("Enter?", false);
        }
    }
}
