using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToyToolboxInteractionManager : MonoBehaviour
{
    //component added to toy object
    public Vector3[] boxOffset, handOffset;
    public bool isInHand, swapHands;
    public HandAnimation handState;
    public ToolboxVacancyChecker boxState;
    public ToyToolboxInteractionManager otherToyState;
    public List<ToolboxVacancyChecker> boxList = new();
    public Collider[] colliders;
   
   

    private void Start()
    {
        FindColliders();
    }
    private void Update()
    {
        if (NewToolboxManager.isOpen)
        {
            FindToyBoxes();
            FindColliders();
            DisableColliders();

        }
       
        
    }


    public void PlaceToyInBox(Collider other)
    { 
        transform.parent = other.transform;
        transform.localPosition = boxOffset[0];
        transform.localRotation = Quaternion.identity;
        if (colliders.Length > 0)
        {
            DisableColliders();
        }
        else return;

    }
    public void StickToyToHand(Collider other, int handID)
    {
        transform.parent = other.transform;
        transform.localPosition = handOffset[handID];
        transform.localRotation = Quaternion.identity;

        if (colliders.Length > 0)
        {
            ActivateColliders();
        }
        else return;
        
        
    }

    void FindToyBoxes()
    {
        
        if (boxList.Count < 1)
        {
            boxList.AddRange(FindObjectsOfType<ToolboxVacancyChecker>());

        }
        else return;
    }

    void FindHand(Collider other)
    {
        handState = other.GetComponent<HandAnimation>();
        
    }

    public void HopToEmptyBox()
    {
        for(int i = 0; i<boxList.Count; i++)
        {
            if (!boxList[i].isOccupied)
            {
                DisableColliders();
                transform.parent = boxList[i].transform;
                transform.localPosition = boxOffset[0];
                transform.localRotation = Quaternion.identity;
                break;
            }
        }

        
    }

    public void FindColliders()
    {
        //only for plane and top
        if(transform.CompareTag("Hopter"))
        {
            colliders.SetValue(GameObject.Find("ThrowHitbox").GetComponent<Collider>(), 0);
        }

        if(transform.CompareTag("SpinningTop"))
        {
            colliders.SetValue(GameObject.Find("ThrowHitbox").GetComponent<Collider>(), 0);
        }
    }

    public void ActivateColliders()
    {
        if(colliders.Length>0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }
        }

      
        
    }

    public void DisableColliders()
    {
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        }

      
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("ToyBox"))
            {
                boxState = other.GetComponent<ToolboxVacancyChecker>();
            }
            if (other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
            {  

                FindHand(other);

                if(isInHand) { swapHands = true; }
                
            }
            if (other.TryGetComponent<KeyItemReporter>(out _)&&isInHand)
            {
                otherToyState = other.GetComponent<ToyToolboxInteractionManager>();

            }
        
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ToyBox"))
        {
            if (isInHand)
            {
                
                if (handState.grip && boxState.isOccupied && boxState.occupantName == transform.name)
                {
                    PlaceToyInBox(other);

                    isInHand = false;
                }
                
            }
        }
        if (other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            if(!isInHand) 
            {
                if(handState.grip)
                {
                    StickToyToHand(other, handState.handID - 1);
                    isInHand = true;
                }
                
            }

            if (isInHand && swapHands)
            {
                if(handState.grip)
                {
                    StickToyToHand(handState.GetComponent<Collider>(), handState.handID - 1);
                    swapHands = false;
                }
                
            }


        }
        if (other.TryGetComponent<KeyItemReporter>(out _) && isInHand)
        {
            if (handState.grip)
            {
                HopToEmptyBox();
                isInHand = false;
                otherToyState.StickToyToHand(handState.GetComponent<Collider>(), handState.handID-1);
                otherToyState.isInHand= true;

            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {
           
            if (isInHand && swapHands)
            {
               
                swapHands = false;
            }


        }
    }

}

