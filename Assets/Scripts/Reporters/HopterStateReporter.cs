using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopterStateReporter : MonoBehaviour
{
    public static bool isInHand, isInBox;
    public EquipVisibilityManager visibilityManager;

    private void Start()
    {
        visibilityManager=GetComponent<EquipVisibilityManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            visibilityManager.isHideable = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("LeftHand")||other.CompareTag("RightHand"))
        {
            isInHand = true;
            isInBox = false;
            visibilityManager.isHideable = false;

            PlayerStateManager.CheckPlayerState();
        }

        if(other.CompareTag("ToySpawn"))
        {
            isInHand = false;
            isInBox = true;
            if (ToolboxManager.instance.isVisible)
                visibilityManager.isHideable = true;

            PlayerStateManager.CheckPlayerState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ToySpawn"))
        {
            visibilityManager.isHideable = false;
        }

        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            visibilityManager.isHideable = false;
        }
    }
}
