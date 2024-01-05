using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewToolboxManager : MonoBehaviour
{
    public static bool isOpen;
    public static List<ToolboxVacancyChecker> checkerList = new();
    
    public Transform leftHand, rightHand;
    public Vector3[] offsetList;
    public int receivedHandID;
    public InputActionReference summonToolboxInputLeft;
    public InputActionReference summonToolboxInputRight;

    void OnEnable()
    {
        summonToolboxInputLeft.action.performed += GetHandIDLeft;
        summonToolboxInputLeft.action.performed += OpenToolBox;
        summonToolboxInputLeft.action.canceled += CloseToolBox;

        summonToolboxInputRight.action.performed += GetHandIDRight;
        summonToolboxInputRight.action.performed += OpenToolBox;
        summonToolboxInputRight.action.canceled += CloseToolBox;

        
    }

    void OpenToolBox(InputAction.CallbackContext boxInput)
    {
        if (boxInput.ReadValue<float>() == 1 && this != null)
        {
            isOpen = true;

            if(checkerList.Count<1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    checkerList.AddRange(GetComponentsInChildren<ToolboxVacancyChecker>());
                }
            }
            

            if (receivedHandID == 1)
            {
                for (int j = 0; j < transform.childCount; j++)
                {
                    transform.GetChild(j).gameObject.SetActive(true);
                }
                this.transform.parent = leftHand;
                this.transform.localPosition = offsetList[0];
                this.transform.localRotation = Quaternion.identity;
            }
            else if (receivedHandID == 2)
            {
                for (int j = 0; j < transform.childCount; j++)
                {
                    transform.GetChild(j).gameObject.SetActive(true);
                }
                this.transform.parent = rightHand;
                this.transform.localPosition = offsetList[1];
                this.transform.localRotation = Quaternion.identity;
            }
        }
        else return;
    }


    void CloseToolBox(InputAction.CallbackContext boxInput)
    {
        if (boxInput.ReadValue<float>() == 0 && this != null)
        {
            isOpen = false;
            for (int j = 0; j < transform.childCount; j++)
            {
                transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        else return;
    }

    public void GetHandIDLeft(InputAction.CallbackContext context) { receivedHandID = 1; }
    public void GetHandIDRight(InputAction.CallbackContext context) { receivedHandID = 2; }
}
