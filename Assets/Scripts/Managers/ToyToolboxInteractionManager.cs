using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ToyToolboxInteractionManager : MonoBehaviour
{
    //component added to toy object
    public Vector3[] boxOffset, handOffset;
    public bool isInHand, swapHands, isInBox;
    public HandAnimation handState, heldHand;
    public ToolboxVacancyChecker boxState;
    public ToyToolboxInteractionManager otherToyState;
    public List<ToolboxVacancyChecker> boxList = new();
    public Collider[] colliders;
    public Renderer[] equipVisuals;
    public AudioSource audioSource;


    public static bool itemTaken;

    public float timeInBox, timeInHand;

   /* private void Awake()
    {
        itemTaken = false;
        
    }*/
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        
        if(isInBox)
        {
            timeInBox+=Time.deltaTime;
            FindColliders();
            DisableColliders();
        }

        if (isInHand)
        {
            timeInHand+=Time.deltaTime;
            FindColliders();
            ActivateColliders();
        }
        
        if (NewToolboxManager.isOpen)
        {
            FindToyBoxes();
            FindColliders();
            DisableColliders();

        }

        TutorialUIToggle();

        if (StoryItemHider.summonToy)
        {
            if (!isInBox && !isInHand)
            {
                ShowEquipVisuals();
            }
            else return;
        }

    }


    public void PlaceToyInBox(ToolboxVacancyChecker boxParent)
    {
        transform.SetParent(boxParent.transform);
        transform.localPosition = boxOffset[0];
        transform.localRotation = Quaternion.Euler(boxOffset[1]);
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
        heldHand = handState;
        handState.handNotEmpty = true;
        handState.reloadCheck = true;
        FindColliders();
        ActivateColliders();

        if (!itemTaken)
        {
            itemTaken = true;
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
                isInBox = true; isInHand = false;
                break;
            }

            if(!NewToolboxManager.isOpen)
            {
                HideEquipVisuals();
            }
        }

        
    }

    public void FindColliders()
    {
        //only for plane and top
        if(transform.CompareTag("Hopter"))
        {
            colliders[0] = GameObject.Find("ThrowHitboxPlane").GetComponent<Collider>();
        }

       else if(transform.CompareTag("SpinningTop"))
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

    void KeepHand()
    {
        handState = heldHand;
       
    }

    void TutorialUIToggle()
    {
            if (TutorialsManager.isTut && !isInBox &&handState!=null)
            {

                    if (transform.CompareTag("SpinningTop") && CustomTopManager.isSpinning)
                    {
                        handState.retractTut.SetActive(true);
                    }
                    else
                    {
                        handState.retractTut.SetActive(false);
                    }
                
            }
            else
            {
            if (handState != null)
            {
                handState.retractTut.SetActive(false);
                handState.shootTut.SetActive(false);
            }
            else return;
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
            if (isInHand && handState != null)
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
            
            if(!isInHand&&handState!=null) 
            {
               
                if(handState.grip)
                {
                    handState.grip = false;
                    print("place in hand");
                    StickToyToHand(handState, handState.handID);
                    audioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[5],0.5f);
                   
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

            if (isInHand && swapHands && handState != null)
            {
                if(handState.grip)
                {
                    handState.grip = false;
                    StickToyToHand(handState, handState.handID);
                    swapHands = false;
                }
                
            }

           

        }

        if (other.TryGetComponent(out ToyToolboxInteractionManager otherItem) && isInHand && otherItem.isInBox && handState != null)
        {
            otherToyState = otherItem;

            if (handState.grip)
            {
                handState.grip = false;
                HopToEmptyBox();
                
                otherToyState.StickToyToHand(handState, handState.handID);
                

            }
        }

        if (other.TryGetComponent<StoryItemHider>(out _)&&DialogueManager.isStory)
        {
            if (!isInHand && !isInBox)
            {
                if (StoryItemHider.summonToy)
                {
                    ShowEquipVisuals();
                }
                else
                {
                    HideEquipVisuals();
                }
            }
            else return;

        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {

            KeepHand();
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

