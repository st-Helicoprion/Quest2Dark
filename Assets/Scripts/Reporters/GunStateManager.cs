using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStateManager : MonoBehaviour
{
    public static bool isInHand, isInBox;
    public EquipVisibilityManager visibilityManager;

    // Start is called before the first frame update
    void Start()
    {
        visibilityManager = GetComponent<EquipVisibilityManager>();
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
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            isInHand = true;
            isInBox = false;
            visibilityManager.isHideable = false;
        }

        if (other.CompareTag("ToySpawn"))
        {
            isInHand = false;
            isInBox = true;
            if (ToolboxManager.instance.isVisible)
                visibilityManager.isHideable = true;
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
