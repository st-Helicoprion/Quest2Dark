using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CicadaHitboxManager : MonoBehaviour
{
    public List<int> hitboxIDList = new List<int>();
    public Collider[] hitboxes;
    public bool isSonarUp;
    public float countToResetSonar;

    private void Start()
    {
        hitboxes= GetComponentsInChildren<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        DetectCicadaActive();
      
    }

    void DetectCicadaActive()
    {
        if (hitboxIDList.Count >= 1)
        {
            DetectHitboxOrder();

        }
        else countToResetSonar+=Time.deltaTime;

        if (countToResetSonar > 1.5f)
        {
            ResetCicada();
            countToResetSonar = 0;
        }
    }

    void DetectHitboxOrder()
    {

        if(hitboxIDList.Count < 4)
        {
            countToResetSonar += Time.deltaTime;
        }

        if (hitboxIDList.Count>=2)
        {
            if (hitboxIDList[0] == hitboxIDList[1])
            {
                ResetCicada();

            }
        }

        if (hitboxIDList.Count > 4)
        {
            
            if (Mathf.Abs(hitboxIDList[1] - hitboxIDList[2]) == 1 && hitboxIDList[0] == hitboxIDList[3])
            {
                isSonarUp= true;
                ClearHitboxList();
                countToResetSonar = 0;
            }
            else
            {
                ClearHitboxList();
                
            }

        }
        else return;
    }

    void ClearHitboxList()
    {
        hitboxIDList.Clear();
    }
       
    void ResetCicada()
    {
        if (isSonarUp)
        {
            isSonarUp = false;

        }
        else return;
    }
  
}
