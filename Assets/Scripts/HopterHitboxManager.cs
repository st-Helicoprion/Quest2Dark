using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HopterHitboxManager : MonoBehaviour
{
    public Material enemyFoundMaterial;
    public Transform enemy;
    public float hopterCount;
    public bool enemyTagged, targetFound;

    private void Start()
    {
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
        transform.GetChild(2).gameObject.SetActive(true);
        
        Vector3 targetDirection = (enemy.transform.position + new Vector3(0, 5, 0)) - transform.position;
        if (!enemyTagged)
        {
            transform.position += 0.2f * Time.deltaTime * targetDirection;
        }
        else hopterCount += Time.deltaTime;


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
            transform.GetChild(2).GetComponent<MeshRenderer>().material = enemyFoundMaterial;
            transform.position = enemy.position + new Vector3(0, 5, 0);
            enemyTagged = true;

        }
      
    }
}
