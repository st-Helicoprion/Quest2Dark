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
    public List<GameObject> promptPool = new();
    public HandAnimation hand;
    public Transform mainCamera;
    private void Start()
    {
        
    }
    private void Update()
    {
        transform.parent.localRotation = new Quaternion(0, mainCamera.localRotation.y, 0, mainCamera.localRotation.w);
        transform.parent.localPosition = new Vector3(0, 1, mainCamera.localPosition.z-0.2f);

        if(promptPool.Count>1)
        {
            ClearPrompts();
        }
    }
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
