using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTrackSensor : MonoBehaviour
{
    public int selfLaneID;
    public static Vector3 playerLaneID;

    private void Start()
    {
        playerLaneID = Vector3.zero;
    }
    private void OnTriggerStay(Collider other)
    {
        if (RunnerMonster.deployed)
        {
            if (other.CompareTag("Player") && selfLaneID == 1)
            {
                RunnerMonster.canShowLanes = true;
                playerLaneID.x = selfLaneID;
                Debug.Log("mine tripped");
            }
            else if (other.CompareTag("Player") && selfLaneID == 2)
            {
                RunnerMonster.canShowLanes = true;
                playerLaneID.y = selfLaneID;
                Debug.Log("mine tripped");
            }
            else if (other.CompareTag("Player") && selfLaneID == 3)
            {
                RunnerMonster.canShowLanes = true;
                playerLaneID.z = selfLaneID;
                Debug.Log("mine tripped");
            }
            else return;
        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (RunnerMonster.deployed)
        {
            if (other.CompareTag("Player") && selfLaneID == 1)
            {

                playerLaneID.x = 0;

            }
            else if (other.CompareTag("Player") && selfLaneID == 2)
            {

                playerLaneID.y = 0;

            }
            else if (other.CompareTag("Player") && selfLaneID == 3)
            {

                playerLaneID.z = 0;

            }
            else return;
        }
        else return;
    }
}
