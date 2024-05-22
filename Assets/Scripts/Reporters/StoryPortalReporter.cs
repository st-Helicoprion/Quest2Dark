using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPortalReporter : MonoBehaviour
{
    public static bool storyStandby;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            storyStandby = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            storyStandby = false;
            StoryItemHider.summonToy = false;
        }
        
    }
}
