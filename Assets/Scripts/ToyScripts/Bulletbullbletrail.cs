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

    void Start()
    {
        rb= GetComponent<Rigidbody>();
        bulletSkin= GetComponent<Renderer>();
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
            StartCoroutine(SonarExpandSequence());
            Destroy(this.gameObject, ExistTime);
        }
        else
        {
            sonarSpawnTime -= Time.deltaTime;
        }


    }

    IEnumerator SonarExpandSequence()
    {
        sonarSpawnTime = sonarDelay;
        int i = 0;
       while(i<6)
        {
            i++;
            GameObject instance = Instantiate(echo, transform.position, Quaternion.identity);
            Destroy(instance, ExistTime);
            yield return new WaitForSeconds(0.2f);
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasHit = true;
        bulletSkin.enabled = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(SonarExpandSequence());
        Destroy(this.gameObject, ExistTime);
    }
}
