using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletbullbletrail : MonoBehaviour
{
    // Start is called before the first frame update
    public float sonarDelay;
    public GameObject echo;
    public float existTime;
    [SerializeField] float speed;
    public GameObject bulletSpawnPoint, bulletSparks;
    Vector3 bulletTrajectory;
    public Rigidbody rb;
    public bool hasHit, spawnSonar;
    public Renderer bulletSkin;
    public AudioSource audioSource;

    void Start()
    {
        rb= GetComponent<Rigidbody>();
        bulletSkin= GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        SaveBulletTrajectory();
    }
    void Update()
    {
        DeployBulletAndSonar();
    }

    void SaveBulletTrajectory()
    {
        bulletSpawnPoint = GameObject.Find("ShootingPoint");
        bulletTrajectory = bulletSpawnPoint.transform.forward;

    }

    void DeployBulletAndSonar()
    {
        if (PlayerMoveFeedback.moving)
        {
            rb.AddForce(600 * speed * bulletTrajectory);
        }
        else
        rb.AddForce(400*speed*bulletTrajectory);

        if (sonarDelay < 0&&!hasHit&&!spawnSonar)
        {
            bulletSkin.enabled = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            SonarExpandSequence();
            
        }
        else
        {
            sonarDelay -= Time.deltaTime;
        }


    }

    void SonarExpandSequence()
    {
        spawnSonar = true;
        int layer = 8;
        int layerMask = 1 << layer;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            bulletSparks.SetActive(true);
            GameObject instance = Instantiate(echo, hit.point-new Vector3(0,60,0), Quaternion.Euler(-90, 0, 0));
            StartCoroutine(RaiseSonarCoroutine(instance));
       
        }
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(AudioManager.instance.ToysSFX[4]);
        }
        
        
       
    }

    IEnumerator RaiseSonarCoroutine(GameObject instance)
    {
        while(instance.transform.position.y<5)
        {
            instance.transform.position += new Vector3(0, 1, 0);
            yield return null;
        }
        Destroy(instance, 1);
        Destroy(this.gameObject, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!spawnSonar)
        {
            hasHit = true;
            bulletSkin.enabled = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            SonarExpandSequence();

        }
       
    }
}
