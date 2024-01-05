using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class PullColliderBehavior : MonoBehaviour
{
    public GameObject pullPrompt;
    public List<GameObject> promptPool = new List<GameObject>();
    public HandAnimation hand;
    public Collider topCollider;
   
    public void ClearPrompts()
    {
        
            promptPool.AddRange(GameObject.FindGameObjectsWithTag("PullPrompt"));
            if (promptPool.Count>0)
            {
                for (int i = 0; i < promptPool.Count; i++)
                {
                    Destroy(promptPool[i]);
                    promptPool.Clear();
                }
               
            }
            else return;
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CustomTopManager.isReadyToSpin)
        {
            if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
            {
                hand = other.GetComponent<HandAnimation>();
                promptPool.Add(Instantiate(pullPrompt, other.transform));
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
      if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
      {
            ClearPrompts();

            if (CustomTopManager.isReadyToSpin&&!hand.handNotEmpty)
            {
                CustomTopManager.isReadyToSpin = false;
                CustomTopManager.isSpinning = true;
                
            }
           
      }
        
    }
}
