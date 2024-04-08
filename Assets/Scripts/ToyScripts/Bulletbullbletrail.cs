using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletbullbletrail : MonoBehaviour
{
    // Start is called before the first frame update
    public float sonarSpawnTime;
    public float sonarDelay;
    public GameObject echo;
    public float ExistTime;
    [SerializeField] float speed;
    public GameObject bulletSpawnPoint;
    Vector3 bulletTrajectory;
    public Rigidbody rb;
    public bool hasHit;
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
        sonarSpawnTime = sonarDelay;
    }

    void DeployBulletAndSonar()
    {
        rb.AddForce(400*speed*bulletTrajectory);

        //transform.position += bulletTrajectory * Time.deltaTime * speed;

        if (sonarSpawnTime < 0&&!hasHit)
        {
            bulletSkin.enabled = false;
            rb.velocity = Vector3.zero;
            SonarExpandSequence();
            Destroy(this.gameObject, 2*ExistTime);
        }
        else
        {
            sonarSpawnTime -= Time.deltaTime;
        }


    }

    void SonarExpandSequence()
    {
        sonarSpawnTime = sonarDelay;
        GameObject instance = Instantiate(echo, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(AudioManager.instance.ToysSFX[4]);
        Destroy(instance, ExistTime);
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasHit = true;
        bulletSkin.enabled = false;
        rb.velocity = Vector3.zero;
        SonarExpandSequence();
        Destroy(this.gameObject, ExistTime);
    }
}
