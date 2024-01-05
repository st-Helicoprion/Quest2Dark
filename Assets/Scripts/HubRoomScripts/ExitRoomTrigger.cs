using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoomTrigger : MonoBehaviour
{   
    public static bool isReadytoPlay;
    public float counter;
    public Animator doorAnim;

    private void Start()
    {
        RestartCount();
    }

    void RestartCount()
    {
        isReadytoPlay = false;
        counter = 1;
    }

    IEnumerator ReadyToBegin()
    {
        isReadytoPlay = true;
        this.GetComponent<Collider>().enabled = false;
        yield return new WaitForEndOfFrame();
        isReadytoPlay = false;
        counter = 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            counter -= Time.deltaTime;

            if (counter <= 0)
            {
                StartCoroutine(ReadyToBegin());
                
            }

        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RestartCount();
            doorAnim.SetTrigger("Close");
        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            doorAnim.SetTrigger("Open");
                //StartCoroutine(ReadyToBegin());

    }
}
