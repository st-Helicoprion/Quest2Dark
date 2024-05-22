using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewToolboxManager : MonoBehaviour
{
    public static bool isOpen;
    public static List<ToolboxVacancyChecker> checkerList = new();
    public List<ToyToolboxInteractionManager> interactorsList = new();
    
    public Transform leftHand, rightHand;
    public Vector3[] offsetList;
    public int receivedHandID;
    public InputActionReference summonToolboxInputLeft;
    public InputActionReference summonToolboxInputRight;

    public Renderer[] boxVisuals;
    public Collider[] colliderList;
    public AudioSource boxAudioSource;

    public GameObject leftGuide, rightGuide;

    void OnEnable()
    {
        summonToolboxInputLeft.action.performed += GetHandIDLeft;
        summonToolboxInputLeft.action.performed += OpenToolBox;
        summonToolboxInputLeft.action.canceled += CloseToolBox;

        summonToolboxInputRight.action.performed += GetHandIDRight;
        summonToolboxInputRight.action.performed += OpenToolBox;
        summonToolboxInputRight.action.canceled += CloseToolBox;

        
    }

    void Start()
    {
        for (int j = 0; j < transform.childCount-2; j++)
        {
            boxVisuals[j].enabled = false;
            colliderList[j].enabled = false;

        }
    }
    void OpenToolBox(InputAction.CallbackContext boxInput)
    {
        if (boxInput.ReadValue<float>() == 1 && this != null)
        {
            isOpen = true;
            boxAudioSource.pitch = 1.2f;
            boxAudioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[1]);

            if(checkerList.Count<1)
            {
                for (int i = 0; i < transform.childCount - 2; i++)
                {
                    checkerList.AddRange(GetComponentsInChildren<ToolboxVacancyChecker>());
                }
            }

            interactorsList.AddRange(FindObjectsOfType<ToyToolboxInteractionManager>());

            if (receivedHandID == 1)
            {
                for (int j = 0; j < transform.childCount-2; j++)
                {

                    boxVisuals[j].enabled = true;
                    colliderList[j].enabled = true;
                  
                }

                for(int k =0;k<interactorsList.Count;k++)
                {
                    if (interactorsList[k].isInBox)
                    {
                        interactorsList[k].ShowEquipVisuals();

                    }
                    else continue;
                }
                this.transform.parent = leftHand;
                this.transform.localPosition = offsetList[0];
                this.transform.localRotation = Quaternion.identity;
                leftGuide.SetActive(true);
                rightGuide.SetActive(false);


            }
            else if (receivedHandID == 2)
            {
                for (int j = 0; j < transform.childCount - 2; j++)
                {
                    boxVisuals[j].enabled = true;
                    colliderList[j].enabled = true;
                }
                for (int k = 0; k < interactorsList.Count; k++)
                {
                    if (interactorsList[k].isInBox)
                    {
                        interactorsList[k].ShowEquipVisuals();

                    }
                    else continue;
                }
                this.transform.parent = rightHand;
                this.transform.localPosition = offsetList[1];
                this.transform.localRotation = Quaternion.identity;
                rightGuide.SetActive(true);
                leftGuide.SetActive(false);
            }
        }
        else return;
    }


    void CloseToolBox(InputAction.CallbackContext boxInput)
    {
        if (boxInput.ReadValue<float>() == 0 && this != null)
        {
            isOpen = false;
            rightGuide.SetActive(false);
            leftGuide.SetActive(false);
            boxAudioSource.pitch = 0.8f;
            boxAudioSource.PlayOneShot(AudioManager.instance.UISFXAudioClips[1]);

            for (int j = 0; j < transform.childCount - 2; j++)
            {
                boxVisuals[j].enabled = false;
                colliderList[j].enabled = false;

            }
            for (int k = 0; k < interactorsList.Count; k++)
            {
                if (interactorsList[k].isInBox)
                {
                    interactorsList[k].HideEquipVisuals();

                }
                interactorsList[k].timeInBox = interactorsList[k].timeInHand = 0;
            }
        }
        else return;
    }

    public void GetHandIDLeft(InputAction.CallbackContext context) { receivedHandID = 1; }
    public void GetHandIDRight(InputAction.CallbackContext context) { receivedHandID = 2; }
}
