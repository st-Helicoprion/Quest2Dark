using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSonarManager : MonoBehaviour
{
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] Transform BulletSpawnPoint;
    [SerializeField] float ExistTime = 1;
    [SerializeField] float TimeInterval = 0.5f;
    GameObject bulletTemp;
    public InputActionReference shootSonarRight;
    public InputActionReference shootSonarLeft;
    public bool isHeld;
    public int handID;
    public HandAnimation handState;
    

    private void Start()
    {
        BulletSpawnPoint = transform.Find("ShootingPoint");      
    }

    private void Update()
    {
        
    }
    void SpawnBullet(InputAction.CallbackContext summonInput)
    {
        if (summonInput.ReadValue<float>() == 1 && this != null && BulletSpawnPoint != null)
            StartCoroutine(SpawnBulletRepeat(BulletSpawnPoint.position, BulletSpawnPoint.rotation));
        else return;
    }

    IEnumerator SpawnBulletRepeat(Vector3 vector3,Quaternion quaternion)
    {
        //SpawnBullet觸發後，每經過TimeInterval的時間，生成一個聲波，總共生SpawnSonar個
            bulletTemp = GameObject.Instantiate(BulletPrefab, vector3,quaternion);
            Debug.Log("spawn");
            Destroy(bulletTemp, ExistTime);
            yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LeftHand")||other.CompareTag("RightHand"))
        {
            handState = other.GetComponent<HandAnimation>();
            handID = handState.handID;
           
        }
       
       
    }

    private void OnTriggerStay(Collider other)
    {
        if(handID == 1)
        {
            if(handState.grip)
            {
                shootSonarLeft.action.performed += SpawnBullet;
                shootSonarRight.action.performed -= SpawnBullet;
            }
        }
        else if (handID == 2)
        {
            if (handState.grip)
            {
                shootSonarLeft.action.performed -= SpawnBullet;
                shootSonarRight.action.performed += SpawnBullet;
            }
        }
        else if (other.CompareTag("ToyBox"))
        {
            handID = 0;
            shootSonarLeft.action.performed -= SpawnBullet;
            shootSonarRight.action.performed -= SpawnBullet;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftHand")&&handID==1)
        {
            shootSonarLeft.action.performed -= SpawnBullet;
            shootSonarRight.action.performed -= SpawnBullet;
            isHeld = false;
        }
        else if (other.CompareTag("RightHand")&&handID==2)
        {
            shootSonarLeft.action.performed -= SpawnBullet;
            shootSonarRight.action.performed -= SpawnBullet;
            isHeld = false;
        }
    }
}
