using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryItemHider : MonoBehaviour
{
    public static bool summonToy = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            if (summonToy)
            {
                this.gameObject.SetActive(false);
            }
            else return;
        }
    }
}
