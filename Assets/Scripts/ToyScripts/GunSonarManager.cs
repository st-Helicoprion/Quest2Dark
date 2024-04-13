using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSonarManager : MonoBehaviour
{
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] Transform BulletSpawnPoint;
    GameObject bulletTemp;
    public InputActionReference shootSonarRight;
    public InputActionReference shootSonarLeft;
    public bool isHeld;
    public int handID;
    public HandAnimation handState, heldHand;
    public AudioSource gunAudioSource;
    public float gunDelay;
    public ToyToolboxInteractionManager toolboxInteractionManager;

    private void Update()
    {
        gunDelay -= Time.deltaTime;

        if(!NewToolboxManager.isOpen&&toolboxInteractionManager.isInHand)
        {
            CheckHeldHand();
        }

        if(!NewToolboxManager.isOpen&&!toolboxInteractionManager.isInHand)
        {
            shootSonarLeft.action.performed -= SpawnBullet;
            shootSonarRight.action.performed -= SpawnBullet;
        }
        
    }
    void SpawnBullet(InputAction.CallbackContext summonInput)
    {
        if (summonInput.ReadValue<float>() == 1 && this != null && BulletSpawnPoint != null)
        {
            if(gunDelay<0)
            {
                gunAudioSource.pitch = Random.Range(1.7f, 2.1f);
                gunAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[1]);
                StartCoroutine(SpawnBulletRepeat(BulletSpawnPoint.position, BulletSpawnPoint.rotation));
            }
           
        }
        else return;
    }

    IEnumerator SpawnBulletRepeat(Vector3 vector3,Quaternion quaternion)
    {
        //SpawnBullet觸發後，每經過TimeInterval的時間，生成一個聲波，總共生SpawnSonar個
            bulletTemp = GameObject.Instantiate(BulletPrefab, vector3,quaternion);
            gunDelay = 1;
            yield return null;
    }

    void KeepHand()
    {
        handState = heldHand;
        handID = handState.handID;
    }

    void CheckHeldHand()
    {
        if (handID == 0)
        {
            shootSonarLeft.action.performed += SpawnBullet;
            shootSonarRight.action.performed -= SpawnBullet;
        }
        else if (handID == 1)
        {
            shootSonarLeft.action.performed -= SpawnBullet;
            shootSonarRight.action.performed += SpawnBullet;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            handState = other.GetComponent<HandAnimation>();
            handID = handState.handID;
        }
        if (other.CompareTag("ToyBox") && NewToolboxManager.isOpen)
        {
            shootSonarLeft.action.performed -= SpawnBullet;
            shootSonarRight.action.performed -= SpawnBullet;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ToyBox"))
        {
            shootSonarLeft.action.performed -= SpawnBullet;
            shootSonarRight.action.performed -= SpawnBullet;

        }

        if (handState != null)
        {
            if (handID == 0 && handState.grip)
            {
                shootSonarLeft.action.performed += SpawnBullet;
                shootSonarRight.action.performed -= SpawnBullet;
                heldHand = handState;   
            }

            if (handID == 1 && handState.grip)
            {
                shootSonarLeft.action.performed -= SpawnBullet;
                shootSonarRight.action.performed += SpawnBullet;
                heldHand = handState;
            }
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if(heldHand!= null)
        {
            if (other.CompareTag("LeftHand") && heldHand.transform.CompareTag("RightHand"))
            {
                KeepHand();
                CheckHeldHand();
            }

            else if (other.CompareTag("RightHand") && heldHand.transform.CompareTag("LeftHand"))
            {
                KeepHand();
                CheckHeldHand();
            }
        }
    
    }



}
