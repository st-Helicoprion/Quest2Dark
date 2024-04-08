using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPlayerSonarReporter : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Sonar")||other.CompareTag("PlaneSonar")||other.CompareTag("TopSonar")||other.CompareTag("GunSonar"))
        {
            if(RunnerMonster.canShowLanes)
            {
                RunnerMonster.ShowAttackLane();

            }
        }
    }
}
