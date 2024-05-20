using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomReporter : MonoBehaviour
{
    public static bool startTheEnd;
    public Renderer spotSkin;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!EndingManager.instance.isPlayingEnd)
            {
                startTheEnd= true;
                spotSkin.enabled = false;
            }
        }
    }
}
