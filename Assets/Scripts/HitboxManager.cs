using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    public List<int> hitboxIDList = new List<int>();
    public bool isSonarUp;
    

    // Update is called once per frame
    void Update()
    {
        if (hitboxIDList.Count >= 1)
        {
            DetectHitboxOrder();
            
        }
        else Debug.Log("no hits");

    }



    void DetectHitboxOrder()
    {
        StartCoroutine(DelayToResetCicada());
        if (hitboxIDList.Count > 4)
        {
            
            if (Mathf.Abs(hitboxIDList[1] - hitboxIDList[2]) == 1 && hitboxIDList[0] == hitboxIDList[3])
            {
                isSonarUp= true;
                ClearHitboxList();
                
            }
            else
            {
                ClearHitboxList();
                isSonarUp = false;

            }

        }
        else return;
    }

    void ClearHitboxList()
    {
        hitboxIDList.Clear();
    }
       
    IEnumerator DelayToResetCicada()
    {
        if(isSonarUp)
        {
            yield return new WaitForSeconds(3);
            isSonarUp = false;

        }
    }
  
}
