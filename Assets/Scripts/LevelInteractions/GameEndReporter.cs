using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEndReporter : MonoBehaviour
{
    public Transform towerObj;
    public static bool tutorialDone, callTower;

    private void Start()
    {
        PrizeBoxManager.taken = false;

        if(PlayerPrefs.HasKey("IntroDone"))
        {
            tutorialDone = true;
        }
    }
    public void Update()
    {
        
        if(callTower)
        {
            callTower = false;
            StartCoroutine(SummonBrain());
        }
        
    }

    public IEnumerator SummonBrain()
    {
        Vector3 towerTargetPos = towerObj.localPosition;
        while(towerObj.localPosition.y<24)
        {
            towerTargetPos.y += 0.5f;
            towerTargetPos.x= towerObj.localPosition.x;
            towerTargetPos.z= towerObj.localPosition.z;
            towerObj.localPosition = towerTargetPos;
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.win = true;
        }
    }
}
