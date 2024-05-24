using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DebugHelper : MonoBehaviour
{
    public InputActionReference debug;

    public GameObject[] testObjects;

    public GameObject[] extraObjects;

    public GameObject[] unlocks;

    // Start is called before the first frame update
    void Awake()
    {
        

       //debug.action.performed += EnableTestBool;

    }

    private void Start()
    {
        debug.action.performed += EnableTestObjects;
        debug.action.canceled += DisableTestObjects;
    }

    private void Update()
    {
        if (TutorialsManager.cicadaTut)
        {
            unlocks[0].SetActive(true);
        }
        if(TutorialsManager.planeTut)
        {
            unlocks[1].SetActive(true);
        }
        if(TutorialsManager.topTut)
        {
            unlocks[2].SetActive(true);
        }
    }


    /*   void EnableTestBool(InputAction.CallbackContext obj)
       {
           if (this != null)
           {
               if (obj.ReadValue<float>() == 1)
               {
                   if (!PrizeBoxManager.taken)
                       PrizeBoxManager.taken = true;
                   else PrizeBoxManager.taken = false;
               }
           }
       }*/

    void EnableTestObjects(InputAction.CallbackContext obj)
    {
        if(this!=null)
        {
            if (obj.ReadValue<float>() == 1)
            {
                if (testObjects.Length > 0)
                {
                    extraObjects[0].SetActive(false);
                    for (int i = 0; i < testObjects.Length; i++)
                    {
                        testObjects[i].SetActive(true);
                        
                    }
                }
                else return;

            }
        }
       
    }

    void DisableTestObjects(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            if (obj.ReadValue<float>() == 0)
            {
                if (testObjects.Length > 0)
                {
                    extraObjects[0].SetActive(true);
                    for (int i = 0; i < testObjects.Length; i++)
                    {
                        testObjects[i].SetActive(false);
                      
                    }
                }
                else return;

            }
        }
    }
    
}
