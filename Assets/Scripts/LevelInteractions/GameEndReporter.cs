using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEndReporter : MonoBehaviour
{
    public Transform towerObj;
    public Transform[] enemySpawnSonar;
    public static bool tutorialDone, callTower;
    public Renderer[] sonarSkin;
    public GameObject winMessage;

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Start()
    {
        PrizeBoxManager.taken = false;

       /* if(PlayerPrefs.GetInt("IntroDone") == 1)
        {
            tutorialDone = true;
        }*/
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
            StartCoroutine(EnemySpawnFeedback(1));
        }

        if (GameManager.instance.sendForceSignal)
        {
            GameManager.instance.sendForceSignal = false;
            StartCoroutine(MindBreakSonar());
        }
     
    }

    public IEnumerator SummonBrain()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[0]);
        }
        
        Vector3 towerTargetPos = towerObj.localPosition;
        while(towerObj.localPosition.y<40)
        {
            towerTargetPos.y += 0.5f;
            towerTargetPos.x= towerObj.localPosition.x;
            towerTargetPos.z= towerObj.localPosition.z;
            towerObj.localPosition = towerTargetPos;
            yield return null;
        }
    }

    public IEnumerator EnemySpawnFeedback(int speed)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[1]);
        }
        enemySpawnSonar[speed].localScale = new Vector3(.1f,.1f,.1f);
        foreach(Renderer renderer in sonarSkin) 
        { 
            renderer.enabled = true;
        }
        yield return null;
        Vector3 enemySpawnFeedbackSize = enemySpawnSonar[0].localScale;
        while (enemySpawnSonar[speed].localScale.y < 1000)
        {
            enemySpawnFeedbackSize += speed*new Vector3(.5f, .5f, .5f);
            enemySpawnSonar[speed].localScale= enemySpawnFeedbackSize;
            yield return null;
        }
        yield return null;
        foreach (Renderer renderer in sonarSkin)
        {
            renderer.enabled = false;
            
        }
    }

    public IEnumerator MindBreakSonar()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClips[2]);
        }
        for (int i = 0;i<5;i++)
        {
            StartCoroutine(EnemySpawnFeedback(i));
            yield return null;
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.win = true;
            winMessage.SetActive(true);
        }
    }
}
