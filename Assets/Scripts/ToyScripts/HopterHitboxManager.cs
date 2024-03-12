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
    public AudioSource planeAudioSource;

    private void Start()
    {
       playerCam = GameObject.Find("Main Camera").transform;
       SetFlightDirection();
        planeAudioSource.pitch = Random.Range(1, 1.3f);
        planeAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[0]);
    }

    private void Update()
    {

        if (!enemyTagged)
        {
            HopterMoveForward();
        }
       
        if(enemyTagged)

        {
            HopterTracking();
        }

    }

    void SetFlightDirection()
    {
        transform.parent.forward = playerCam.forward;
    }

    void HopterMoveForward()
    {
        HopterSonarSummon();
        hopterLifeCount += Time.deltaTime;

        transform.parent.position += 4 * Time.deltaTime * transform.parent.forward;

        if (hopterLifeCount > 10)
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

        if (visualAidCount >0.5f)
        {
            visualAidCount = 0;
            if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit, Mathf.Infinity))
            {
                if(hit.transform.CompareTag("Ground"))
                {
                    Instantiate(hopterSonarPrefab, new Vector3(transform.position.x,hit.point.y,transform.position.z), Quaternion.identity);
                    Debug.Log("plane sonar released");
                    
                }
            }
            
        }
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger"))
        {
            enemy = other.transform;
            enemyTagged = true;
            Debug.Log("enemy tagged, tracking start");
            hopterLifeCount = 0;
        }
      
    }


}
