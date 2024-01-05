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
    public float MaxDistance;
    [SerializeField] float speed;
    public GameObject bulletSpawnPoint;
    Vector3 bulletTrajectory;

    void Start()
    {
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
        transform.position += bulletTrajectory * Time.deltaTime * speed;
        MaxDistance -= Time.deltaTime * speed;
        if (MaxDistance < 0)
        {
            Destroy(this.gameObject);

        }
           
        if (sonarSpawnTime <= 0)
        {
            GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
            Destroy(instance, ExistTime);
            sonarSpawnTime = sonarDelay;
        }
            
        else
        {
            sonarSpawnTime -= Time.deltaTime;
        }

        
    }
}
