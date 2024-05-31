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
    public bool enemyTagged, targetFound, wallHit;
    public AudioSource planeAudioSource;
    public Animator anim;

    private void Start()
    {
       playerCam = GameObject.Find("Main Camera").transform;
       SetFlightDirection();
        planeAudioSource.pitch = Random.Range(1, 1.3f);
        planeAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[0]);
        anim.SetBool("Flying", true);
    }

    private void Update()
    {
        if (!wallHit)
        {
            if (enemyTagged)
            {
                HopterTracking();
            }
            else HopterMoveForward();
        }
        else return;

        if(!enemyTagged)
        {
            if (hopterLifeCount > hopterLifetime)
            {
                hopterLifeCount = 0;
                Destroy(transform.parent.gameObject);
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

        if(PlayerMoveFeedback.moving)
        {
            transform.parent.position += 8 * Time.deltaTime * transform.parent.forward;
        }else
        transform.parent.position += 4 * Time.deltaTime * transform.parent.forward;


        hopterLifetime = 10;
        
    }

    void HopterTracking()
    {
        HopterPlaceTracker();
        if (enemy != null)
        {
            Vector3 targetDirection = (enemy.transform.position + new Vector3(0, 5, 0)) - transform.position;
            if (Mathf.Abs(enemy.transform.position.x - transform.position.x) > 0.25f || Mathf.Abs(enemy.transform.position.z - transform.position.z) > 0.25f)
            {
                transform.parent.position += 2 * Time.deltaTime * targetDirection;
            }


            hopterLifeCount += Time.deltaTime;

            hopterLifetime = 120;

        }
        else Destroy(transform.parent.gameObject);
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
                Instantiate(hopterSonarPrefab, new Vector3(transform.position.x, hit.point.y - 60, transform.position.z), Quaternion.identity);
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

    IEnumerator ErasePlane()
    {
        yield return new WaitForSeconds(1);
        Destroy(transform.parent.gameObject);
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
           
        if(other.CompareTag("Ground"))
        {
            wallHit= true;
            Rigidbody parentRB = transform.parent.GetComponent<Rigidbody>();
            Collider parentColl = transform.parent.GetComponent<Collider>();
            parentColl.isTrigger = false;
            parentRB.isKinematic = false;
            StartCoroutine(ErasePlane());
        }
    }


}
