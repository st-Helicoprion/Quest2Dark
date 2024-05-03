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

    public bool debugObjsActive;

    // Start is called before the first frame update
    void Awake()
    {
        debugObjsActive = false;
        //debug.action.performed += EnableTestObjects;
        debug.action.performed += EnableTestBool;

    }

    void EnableTestBool(InputAction.CallbackContext obj)
    {
        if (this != null)
        {
            if (obj.ReadValue<float>() == 1)
            {
                PrizeBoxManager.taken = true;
            }
        }
    }

    void EnableTestObjects(InputAction.CallbackContext obj)
    {
        if(this!=null)
        {
            if (obj.ReadValue<float>() == 1 && !debugObjsActive)
            {
                if (testObjects.Length > 0)
                {
                    for (int i = 0; i < testObjects.Length; i++)
                    {
                        testObjects[i].SetActive(true);
                        debugObjsActive = true;
                    }
                }
                else return;

            }
        }
       
    }
}
