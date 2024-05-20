using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class BGMSwitcherReporter : MonoBehaviour
{
    public int selfAreaID;
    public static int currentAreaID;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (currentAreaID != selfAreaID&&!WeakStateManager.instance.weakened)
            {
                currentAreaID = selfAreaID;
                AudioManager.instance.CheckBGMToPlay();
            }
            else return;
        }
    }

}
