using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PullColliderBehavior : MonoBehaviour
{
    public GameObject pullPrompt;
    public List<GameObject> promptPool = new List<GameObject>();
    public Collider pullCollider;

    private void Start()
    {
        pullCollider= GetComponent<Collider>();
    }

    private void Update()
    {
        CheckColliderActive();
    }


    void CheckColliderActive()
    {
        if (!pullCollider.enabled)
        {
            if (promptPool.Count>0)
            {
                for (int i = 0; i < promptPool.Count; i++)
                {
                    Destroy(promptPool[i]);
                    
                }
                Debug.Log("pull prompts cleaned");
            }
            else return;
        }
        else return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
           promptPool.Add(Instantiate(pullPrompt, other.transform));
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

            for(int i = 0; i<promptPool.Count; i++)
            {
                Destroy(promptPool[i]);
                promptPool.Clear();
            }

            /*if(other.transform.Find("PullPrompt(Clone)")!=null)
            {
                Destroy(other.transform.Find("PullPrompt(Clone)").gameObject);
            }*/
            
        }
    }
}
