using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEndReporter : MonoBehaviour
{
    public Transform towerObj, enemySpawnSonar;
    public static bool tutorialDone, callTower;
    public Renderer[] sonarSkin;

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

        if (GameManager.enemySpawned)
        {
            GameManager.enemySpawned = false;
            StartCoroutine(EnemySpawnFeedback());
        }

    }

    public IEnumerator SummonBrain()
    {
        Vector3 towerTargetPos = towerObj.localPosition;
        while(towerObj.localPosition.y<35)
        {
            towerTargetPos.y += 0.5f;
            towerTargetPos.x= towerObj.localPosition.x;
            towerTargetPos.z= towerObj.localPosition.z;
            towerObj.localPosition = towerTargetPos;
            yield return null;
        }
    }

    public IEnumerator EnemySpawnFeedback()
    {
        enemySpawnSonar.localScale = new Vector3(.1f,.1f,.1f);
        foreach(Renderer renderer in sonarSkin) 
        { 
            renderer.enabled = true;
        }
        yield return null;
        Vector3 enemySpawnFeedbackSize = enemySpawnSonar.localScale;
        while (enemySpawnSonar.localScale.y < 1000)
        {
            enemySpawnFeedbackSize += new Vector3(.5f, .5f, .5f);
            enemySpawnSonar.localScale= enemySpawnFeedbackSize;
            yield return null;
        }
        yield return null;
        foreach (Renderer renderer in sonarSkin)
        {
            renderer.enabled = false;
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
