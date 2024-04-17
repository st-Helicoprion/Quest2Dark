using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSpawnReporter : MonoBehaviour
{
    public static Transform player;
    public float distToPlayer;
   
    // Update is called once per frame
    void Update()
    {
        if(player!=null)
        {
            distToPlayer = Vector3.Distance(transform.position, player.position);
        }

        if (distToPlayer < 10)
        {
            TutorialsManager.spawnPointToUse = this;
        }
        else return;
    }
        
}
