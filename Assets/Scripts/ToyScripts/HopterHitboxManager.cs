using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HopterHitboxManager : MonoBehaviour
{
    public GameObject hopterSonarPrefab;
    //public Material enemyFoundMaterial;
    public Transform enemy, playerCam;
    public float hopterLifeCount, visualAidCount;
    public bool enemyTagged, targetFound;

    private void Start()
    {
       playerCam = GameObject.Find("Main Camera").transform;
       //StartCoroutine(FindUntaggedEnemy());
    }

    private void Update()
    {
        //command to auto-track enemies
        /* if (targetFound)
         {
             HopterMoveToTarget();
         }*/
        /*if (enemyTagged&&this.gameObject.IsDestroyed())
        {
            HopterManager.untaggedEnemies.Add(enemy.gameObject);
            Debug.Log("enemy returned to list forcefully");
        }*/

        if (!enemyTagged)
            HopterMoveForward();
        else
            HopterTracking();

    }

    IEnumerator FindUntaggedEnemy()
    {
        //code component to auto-track enemies [IGNORE]
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

    void HopterMoveForward()
    {
        HopterSonarSummon();
        hopterLifeCount += Time.deltaTime;

        transform.forward = playerCam.forward;
        transform.position += 8 * Time.deltaTime * transform.forward;

        if (hopterLifeCount > 20)
        {
            hopterLifeCount = 0;
            Destroy(this.gameObject);
        }
    }

    void HopterTracking()
    {
        HopterSonarSummon();
        Vector3 targetDirection = (enemy.transform.position + new Vector3(0, 5, 0)) - transform.position;
        if(Mathf.Abs(enemy.transform.position.x-transform.position.x)>0.25f|| Mathf.Abs(enemy.transform.position.z - transform.position.z) > 0.25f)
        {
            transform.LookAt(targetDirection);
            transform.position += Time.deltaTime * targetDirection;
        }
       

        hopterLifeCount += Time.deltaTime;

        if (hopterLifeCount > 60)
        {
            hopterLifeCount = 0;
            Destroy(this.gameObject);
        }
    }

    void HopterSonarSummon()
    {
        visualAidCount += Time.deltaTime;

        if (visualAidCount > 1)
        {
            Instantiate(hopterSonarPrefab, transform.position, Quaternion.identity);
            Debug.Log("plane sonar released");
            visualAidCount = 0;
        }
    }

    void HopterMoveToTarget()
    {
        //code component to auto-track enemies [IGNORE]
        visualAidCount += Time.deltaTime;

        if (visualAidCount > 2)
        {
            Vector3 targetDirection = (enemy.transform.position + new Vector3(0, 5, 0)) - transform.position;

            if (!enemyTagged)
            {
                transform.LookAt(targetDirection);
                transform.position += 5*Time.deltaTime * targetDirection.normalized;
            }
            else hopterLifeCount += Time.deltaTime;

        }
        else
        {
            transform.forward= playerCam.forward;
            transform.position += 5 * Time.deltaTime * playerCam.forward;
        }


            if (hopterLifeCount > 60)
        {
            hopterLifeCount = 0;
            if (enemyTagged)
            {
                HopterManager.untaggedEnemies.Add(enemy.gameObject);
                Debug.Log("enemy returned to list");
            }
            Destroy(this.gameObject);
        }

    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            //transform.GetChild(1).GetComponent<MeshRenderer>().material = enemyFoundMaterial;
            enemy = other.transform;
            enemyTagged = true;
            Debug.Log("enemy tagged, tracking start");
            hopterLifeCount = 0;
        }
      
    }


}
