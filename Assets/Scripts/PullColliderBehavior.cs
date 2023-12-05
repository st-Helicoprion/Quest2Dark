using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullColliderBehavior : MonoBehaviour
{
    public GameObject pullPrompt;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            Instantiate(pullPrompt, other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("LeftHand")||other.CompareTag("RightHand"))
        {
            TopSonarManager.isReadyToSpin = false;
            TopSonarManager.isSpinTop= true;
            TopSonarManager.isInBox = false;
            TopSonarManager.isInHand = false;

            if(other.transform.Find("PullPrompt(Clone)")!=null)
            {
                Destroy(other.transform.Find("PullPrompt(Clone)").gameObject);
            }
            
        }
    }
}
