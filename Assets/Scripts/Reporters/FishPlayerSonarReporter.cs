using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPlayerSonarReporter : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Sonar")||other.CompareTag("PlaneSonar")||other.CompareTag("TopSonar")||other.CompareTag("GunSonar"))
        {
            if(RunnerMonster.canShowLanes&&!RunnerMonster.switching)
            {
                if (RunnerMonster.laneID== FishTrackSensor.playerLaneID.x)
                {
                    RunnerMonster.ShowAttackLane();
                }
                else if(RunnerMonster.laneID == FishTrackSensor.playerLaneID.y)
                {
                    RunnerMonster.ShowAttackLane();
                }
                else if (RunnerMonster.laneID == FishTrackSensor.playerLaneID.z)
                {
                    RunnerMonster.ShowAttackLane();
                }
               

            }
        }
    }
}
