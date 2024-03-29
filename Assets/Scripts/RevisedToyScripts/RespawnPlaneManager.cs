using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnPlaneManager : MonoBehaviour
{
    public int planeMaxNum;
    public Transform mainCamera;
    public List<GameObject> planesInScene = new();
    public List<GameObject> planesOnHand = new();
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

        sonarScript.enabled = true;
        trackHitbox.enabled = true;

        planesInScene.Add(other);

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
            planesOnHand.Add(planeInstance);

            if(planesOnHand.Count>1)
            {
                Destroy(planesOnHand[0]);
                planesOnHand.RemoveAt(0);
            }
        }
        else if(boxArray.Length > 0)
        {
            for(int i =0;i< boxArray.Length;i++)
            {
                if (!boxArray[i].isOccupied)
                {
                    planeInstance = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);
                    planeInstance.GetComponent<ToyToolboxInteractionManager>().PlaceToyInBox(boxArray[i]);
                    planesOnHand.Add(planeInstance);

                    if (planesOnHand.Count > 1)
                    {
                        Destroy(planesOnHand[0]);
                        planesOnHand.RemoveAt(0);
                    }
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

        if (other.CompareTag("Hopter") && other.GetComponent<ToyToolboxInteractionManager>().isInHand )
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
        else return;

    }

   
}

