using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTrackSensor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
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

}
