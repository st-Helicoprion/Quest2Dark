using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HopterHitboxManager : MonoBehaviour
{
    public GameObject hopterSonarPrefab, trackerLight;
    public Transform enemy, playerCam;
    public float hopterLifeCount, visualAidCount, hopterLifetime;
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
        if(enemyTagged)
        {
            HopterTracking();
        }
        else HopterMoveForward();

        if(!enemyTagged)
        {
            if (hopterLifeCount > hopterLifetime)
            {
                hopterLifeCount = 0;
                Destroy(this.gameObject);
            }

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

        hopterLifetime = 10;
        
    }

    void HopterTracking()
    {
        HopterPlaceTracker();
        Vector3 targetDirection = (enemy.transform.position + new Vector3(0, 5, 0)) - transform.position;
        if(Mathf.Abs(enemy.transform.position.x-transform.position.x)>0.25f|| Mathf.Abs(enemy.transform.position.z - transform.position.z) > 0.25f)
        {
            transform.position += 2*Time.deltaTime * targetDirection;
        }
       

        hopterLifeCount += Time.deltaTime;

        hopterLifetime = 120;
    }

    void HopterSonarSummon()
    {
        visualAidCount += Time.deltaTime;

        if (visualAidCount > 0.7f)
        {
            int layer = 8;
            int layerMask = 1 << layer;

            if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                   Instantiate(hopterSonarPrefab, new Vector3(transform.position.x,hit.point.y-60,transform.position.z), Quaternion.identity);
                    Debug.Log("plane sonar released");
                    visualAidCount = 0;
            }
            
        }
    }

    void HopterPlaceTracker()
    {
        trackerLight.SetActive(true);

        if(!targetFound)
        {
            targetFound = true;
            planeAudioSource.pitch = Random.Range(1, 1.3f);
            planeAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[5]);
        }
        
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger")||other.CompareTag("Runner"))
        {
            enemy = other.transform;
            enemyTagged = true;
            Debug.Log("enemy tagged, tracking start");
            hopterLifeCount = 0;
        }
      
    }


}
