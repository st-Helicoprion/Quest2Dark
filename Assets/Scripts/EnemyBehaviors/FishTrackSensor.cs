using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTrackSensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (RunnerMonster.deployed)
        {
            if (other.CompareTag("Player"))
            {
                RunnerMonster.canShowLanes = true;
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
            if (other.CompareTag("Player"))
            {
                RunnerMonster.canShowLanes = false;
            }
            else return;

        }
        else return;
    }
}
