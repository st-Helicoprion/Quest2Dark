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
        sonarScript = other.transform.GetChild(0).GetComponent<HopterHitboxManager>();
        trackHitbox = other.transform.GetChild(0).GetComponent<Collider>();

        Collider self = GetComponent<Collider>();
        self.enabled = false;

        sonarScript.enabled = true;
        trackHitbox.enabled = true;

        planesInScene.Add(other.gameObject);

        lockPlaneRespawn = true;

        StartCoroutine(DelayToRefresh());
    }

    void RefreshPlane()
    {
        HandAnimation handState = hand.GetComponent<HandAnimation>();
        ToolboxVacancyChecker[] boxArray = NewToolboxManager.checkerList.ToArray();

       
        if(!handState.handNotEmpty)
        {
          planeInstance = Instantiate(planePrefab,Vector3.zero,Quaternion.identity);
            planeInstance.GetComponent<ToyToolboxInteractionManager>().StickToyToHand(hand.GetComponent<Collider>(), handState.handID-1);
        }
        else if(boxArray.Length > 0)
        {
            for(int i =0;i< boxArray.Length;i++)
            {
                if (!boxArray[i].isOccupied)
                {
                    planeInstance = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);
                    planeInstance.GetComponent<ToyToolboxInteractionManager>().PlaceToyInBox(boxArray[i].GetComponent<Collider>());
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


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hopter") && other.GetComponent<ToyToolboxInteractionManager>().isInHand && !lockPlaneRespawn)
        {
            hand = other.GetComponent<ToyToolboxInteractionManager>().handState.gameObject;
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


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hand.tag))
        {
            lockPlaneRespawn = false;
        }
    }

   
}

