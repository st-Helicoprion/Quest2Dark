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
    public Collider trackHitbox, selfCollider;

    private void Start()
    {
        selfCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        transform.parent.localRotation = new Quaternion(0, mainCamera.localRotation.y, 0, mainCamera.localRotation.w);
        transform.parent.localPosition = new Vector3(0, 1, mainCamera.localPosition.z - 0.1f);
    }
    void StartTracking(GameObject other)
    {
        if (other.transform.childCount>0)
        {
            other.transform.parent = null;
            sonarScript = other.transform.GetChild(0).GetComponent<HopterHitboxManager>();
            trackHitbox = other.transform.GetChild(0).GetComponent<Collider>();
        }
        else return;

        sonarScript.enabled = true;
        trackHitbox.enabled = true;

        if(planesOnHand.Count>0)
        {
            planesOnHand.RemoveAt(0);
        }
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
        yield return new WaitForSeconds(4);

        RefreshPlane();

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hopter") && other.GetComponent<ToyToolboxInteractionManager>().isInHand)
        {
            hand = other.GetComponent<ToyToolboxInteractionManager>().handState.gameObject;

            if (planesInScene.Count<=planeMaxNum)
            {
                StartTracking(other.gameObject);
                Debug.Log("plane released");
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

