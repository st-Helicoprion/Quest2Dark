using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlaneManager : MonoBehaviour
{
    public int planeMaxNum;
    public bool lockPlaneRespawn=true;
    public Transform mainCamera;
    public List<GameObject> planesInScene = new();
    public GameObject planePrefab, hand, planeInstance;
    public HopterHitboxManager sonarScript;
    public Collider trackHitbox;

    private void Update()
    {
        transform.parent.localRotation = new Quaternion(0, mainCamera.localRotation.y, 0, mainCamera.localRotation.w);
    }
    void StartTracking(GameObject other)
    {
        other.transform.parent = null;
        sonarScript = other.transform.GetChild(0).GetChild(0).GetComponent<HopterHitboxManager>();
        trackHitbox = other.transform.GetChild(0).GetChild(0).GetComponent<Collider>();

        Collider self = GetComponent<Collider>();
        self.enabled = false;

        sonarScript.enabled = true;
        trackHitbox.enabled = true;

        planesInScene.Add(other);

        lockPlaneRespawn = true;

        StartCoroutine(DelayToRefresh());
    }

    void RefreshPlane()
    {
        HandAnimation handState = hand.GetComponent<HandAnimation>();
        ToolboxVacancyChecker[] boxArray = NewToolboxManager.checkerList.ToArray();

       
        if(!handState.handNotEmpty)
        {
          planeInstance = Instantiate(planePrefab,handState.transform);
            planeInstance.GetComponent<ToyToolboxInteractionManager>().StickToyToHand(handState, handState.handID);
        }
        else if(boxArray.Length > 0)
        {
            for(int i =0;i< boxArray.Length;i++)
            {
                if (!boxArray[i].isOccupied)
                {
                    planeInstance = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);
                    planeInstance.GetComponent<ToyToolboxInteractionManager>().PlaceToyInBox(boxArray[i]);
                    break;
                }
            }
        }
       
    }

    IEnumerator DelayToRefresh()
    {
        yield return new WaitForSeconds(5);

        RefreshPlane();

    }

    IEnumerator CooldownAfterRefresh()
    {
        yield return new WaitForSeconds(2);
        lockPlaneRespawn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hopter") && other.GetComponent<ToyToolboxInteractionManager>().isInHand)
        {
            hand = other.GetComponent<ToyToolboxInteractionManager>().handState.gameObject;

        }
        else return;

        if (other.CompareTag("Hopter") && other.GetComponent<ToyToolboxInteractionManager>().isInHand && !lockPlaneRespawn)
        {

            if (planesInScene.Count < planeMaxNum)
            {
                StartTracking(other.gameObject);
            }
            else
            {
                Destroy(planesInScene[0]);
                planesInScene.RemoveAt(0);
                StartTracking(other.gameObject);
            }


        }
        else return;


    }

    private void OnTriggerExit(Collider other)
    {
        if (hand!=null&&other.CompareTag(hand.tag))
        {
            StartCoroutine(CooldownAfterRefresh());
        }
    }

   
}

