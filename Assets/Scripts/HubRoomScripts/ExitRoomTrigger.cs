using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoomTrigger : MonoBehaviour
{   
    public static bool isReadytoPlay;
    public float counter;

    private void Start()
    {
        RestartCount();
    }

    void RestartCount()
    {
        isReadytoPlay = false;
        counter = 10;
    }

    IEnumerator ReadyToBegin()
    {
        isReadytoPlay = true;
        yield return new WaitForEndOfFrame();
        isReadytoPlay = false;
        counter = 10;
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
            RestartCount();
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(ReadyToBegin());

    }
}
