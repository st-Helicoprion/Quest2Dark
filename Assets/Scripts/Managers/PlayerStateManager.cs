using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static Collider topHitboxThreshold, topPullHitbox, hopterHitbox;
   
    public static void CheckPlayerState()
    {
        topHitboxThreshold = GameObject.Find("TopHitboxThreshold").GetComponent<Collider>();
        topPullHitbox = GameObject.Find("TopHitBox").GetComponent<Collider>();
       
        //top state manager
        if(TopSonarManager.isInBox || TopSonarManager.isSpinTop|| TopSonarManager.isNotFound)
        {
            topPullHitbox.enabled = false;
            topHitboxThreshold.enabled = false;
        }

        if(TopSonarManager.isReadyToSpin)
        {
            topPullHitbox.enabled = true;
            topHitboxThreshold.enabled = false;
        }

        if (TopSonarManager.isInHand)
        {
            topPullHitbox.enabled = false;
            topHitboxThreshold.enabled = true;
        }

        

        hopterHitbox = GameObject.Find("HopterHitbox").GetComponent<Collider>();

        //hopter state manager
        if (HopterStateReporter.isInBox)
        {
            hopterHitbox.enabled = false;
        }

        if(HopterStateReporter.isInHand)
        {
            hopterHitbox.enabled = true;
        }
        else hopterHitbox.enabled = false;

        Debug.Log("player state updated");
    }
}
