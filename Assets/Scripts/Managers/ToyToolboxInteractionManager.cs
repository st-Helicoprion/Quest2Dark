using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToyToolboxInteractionManager : MonoBehaviour
{
    //component added to toy object
    public Vector3[] boxOffset, handOffset;
    public bool isInHand, swapHands, isInBox;
    public HandAnimation handState;
    public ToolboxVacancyChecker boxState;
    public ToyToolboxInteractionManager otherToyState;
    public List<ToolboxVacancyChecker> boxList = new();
    public Collider[] colliders;
    public Renderer[] equipVisuals;

    public float timeInBox, timeInHand;

    private void Update()
    {
        if (NewToolboxManager.isOpen)
        {
            FindToyBoxes();
            FindColliders();
            DisableColliders();

        }
        
        if(isInBox)
        {
            timeInBox+=Time.deltaTime;
            DisableColliders();
        }

        if (isInHand)
        {
            timeInHand+=Time.deltaTime;
            FindColliders();
            ActivateColliders();
        }
        
    }


    public void PlaceToyInBox(ToolboxVacancyChecker boxParent)
    {
        transform.SetParent(boxParent.transform);
        transform.localPosition = boxOffset[0];
        transform.localRotation = Quaternion.identity;
        isInBox= true;isInHand= false;
        FindColliders();
        DisableColliders();
      
    }
    public void StickToyToHand(HandAnimation handParent, int handID)
    {
        
        transform.SetParent(handParent.transform);
        transform.localPosition = handOffset[handID];
        transform.localRotation = Quaternion.identity;
        isInHand= true;isInBox= false;
        handState = handParent;
        FindColliders();
        ActivateColliders();
        
        
        
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

            if(!NewToolboxManager.isOpen)
            {
                HideEquipVisuals();
            }
        }

        
    }

    public void HopToOtherHand()
    {

    }

    public void FindColliders()
    {
        //only for plane and top
        if(transform.CompareTag("Hopter"))
        {
            colliders[0] = GameObject.Find("ThrowHitboxPlane").GetComponent<Collider>();
        }

       if(transform.CompareTag("SpinningTop"))
        {
            colliders[0] = GameObject.Find("ThrowHitboxTop").GetComponent<Collider>();
        }
    }

    public void ActivateColliders()
    {
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
                
            }
        }
        else return;

      
        
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
        else return;

      
    }

    public void ShowEquipVisuals()
    {
        for(int i =0; i<equipVisuals.Length; i++)
        {
            if (equipVisuals[i] != null)
            equipVisuals[i].enabled = true;
        }
    }

    public void HideEquipVisuals()
    {
        for (int i = 0; i < equipVisuals.Length; i++)
        {
            if (equipVisuals[i] != null)
                equipVisuals[i].enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
           
            if (other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
            {  

                FindHand(other);

                if(isInHand) { swapHands = true; }
                
            }

            if (other.TryGetComponent(out ToyToolboxInteractionManager otherItem)&&isInHand)
            {
                otherToyState = otherItem;

            }
        
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out ToolboxVacancyChecker otherBox))
        {
            boxState = otherBox;
            if (isInHand)
            {
                 if (handState.grip)
                    {
                        handState.grip = false;
                        print("put in box");
                        PlaceToyInBox(boxState);

                    }
                
            }
            
            
        }
        if (other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            
            if(!isInHand) 
            {
               
                if(handState.grip)
                {
                    handState.grip = false;
                    print("place in hand");
                    StickToyToHand(handState, handState.handID);
                   
                }

                if (isInBox && timeInBox > timeInHand)
                {
                    if(handState.grip)
                    {
                        handState.grip = false;
                        print("place in hand from box");
                        StickToyToHand(handState, handState.handID);
                    }
                   
                }

            }

            if (isInHand && swapHands)
            {
                if(handState.grip)
                {
                    handState.grip = false;
                    StickToyToHand(handState, handState.handID);
                    swapHands = false;
                }
                
            }


        }

        if (other.TryGetComponent(out ToyToolboxInteractionManager otherItem) && isInHand && otherItem.isInBox)
        {
            otherToyState = otherItem;

            if (handState.grip)
            {
                handState.grip = false;
                HopToEmptyBox();
                
                otherToyState.StickToyToHand(handState, handState.handID);
                

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

        if (other.TryGetComponent<ToolboxVacancyChecker>(out _))
        {
            boxState = null;
        }

        if (other.TryGetComponent<ToyToolboxInteractionManager>(out _) && isInHand)
        {
            otherToyState = null;

        }

      
    }

}

