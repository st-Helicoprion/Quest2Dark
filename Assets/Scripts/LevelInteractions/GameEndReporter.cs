using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEndReporter : MonoBehaviour
{
    public Renderer[] towerSkin;
    public Transform towerObj;
    public float lightCountdown, lightDelay;
    public bool lit, dark, blink;

    private void Start()
    {
        PrizeBoxManager.taken = false;
    }
    public void Update()
    {
        /*lightCountdown -= Time.deltaTime;

        if (lightCountdown<0)
        {
            lightCountdown = lightDelay;
            blink = true;

            if(blink)
            {
                blink= false;
                if(lit)
                {
                    TurnOffLamp();
                }
                else if(dark)
                {
                    TurnOnLamp();
                }
            }
           
        }*/

        if(PrizeBoxManager.taken)
        {
            StartCoroutine(SummonBrain());
        }
        
    }

    /*public  void TurnOnLamp()
    {
        lit = true;
        dark = false;
        for (int i = 0; i < towerSkin.Length; i++)
        {
            towerSkin[i].enabled = true;
        }
    }

    public void TurnOffLamp()
    {
        dark = true;
        lit = false;
        for (int i = 0; i < towerSkin.Length; i++)
        {
            towerSkin[i].enabled = false;
        }
    }*/

    IEnumerator SummonBrain()
    {
        Vector3 towerTargetPos = towerObj.localPosition;
        while(towerObj.localPosition.y<20)
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
