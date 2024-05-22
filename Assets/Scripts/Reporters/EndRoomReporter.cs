using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EndRoomReporter : MonoBehaviour
{
    public static bool startTheEnd;
    public Renderer spotSkin;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            other.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
            other.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
            if (!EndingManager.instance.isPlayingEnd)
            {
                startTheEnd= true;
                spotSkin.enabled = false;
            }
        }
    }
}
