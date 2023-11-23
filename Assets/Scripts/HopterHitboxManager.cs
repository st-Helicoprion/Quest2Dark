using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HopterHitboxManager : MonoBehaviour
{
    public Material enemyFoundMaterial;
    public Transform enemy, playerCam;
    public float hopterCount, visualAidCount;
    public bool enemyTagged, targetFound;

    private void Start()
    {
       playerCam = GameObject.Find("Main Camera").transform;
       StartCoroutine(FindUntaggedEnemy());
    }

    private void Update()
    {
        if (targetFound)
        {
            HopterMoveToTarget();
        }
      

        if (this.gameObject.IsDestroyed())
        {
            HopterManager.untaggedEnemies.Add(enemy.gameObject);
            Debug.Log("enemy returned to list forcefully");
        }
    }

    IEnumerator FindUntaggedEnemy()
    {
        
        if (HopterManager.untaggedEnemies.Count > 0)
        {
            enemy = HopterManager.untaggedEnemies[0].transform;
            yield return null;
            HopterManager.untaggedEnemies.RemoveAt(0);
            targetFound = true;
            Debug.Log("first enemy removed from list");
        }
        else yield break;
    }
    void HopterMoveToTarget()
    {  
        visualAidCount += Time.deltaTime;

        if (visualAidCount > 2)
        {
            Vector3 targetDirection = (enemy.transform.position + new Vector3(0, 5, 0)) - transform.position;
            if (!enemyTagged)
            {
                transform.LookAt(targetDirection);
                transform.position += Time.deltaTime * targetDirection;
            }
            else hopterCount += Time.deltaTime;

        }
        else
        {
            transform.forward= playerCam.forward;
            transform.position += 5 * Time.deltaTime * playerCam.forward;
        }


            if (hopterCount > 100)
        {
            hopterCount = 0;
            HopterManager.untaggedEnemies.Add(enemy.gameObject);
            Debug.Log("enemy returned to list");
            Destroy(this.gameObject);
        }

    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            transform.GetChild(1).GetComponent<MeshRenderer>().material = enemyFoundMaterial;
            transform.position = enemy.position + new Vector3(0, 5, 0);
            enemyTagged = true;

        }
      
    }
}
