using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTopManager : MonoBehaviour
{
    public static bool isReadyToSpin, isSpinning, domainExpanded;
    public GameObject AOESonar, sonarDust, hand;
    public List<Vector3> trackedPositions = new List<Vector3>();
    public float force;
    public Rigidbody rb;
    public Animator anim;
    public float topLifetime, topCounter;
    public ToyToolboxInteractionManager toolboxHelper;
    public HandAnimation handState;

    // Start is called before the first frame update
    void Start()
    {
        topCounter = topLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            ActivateTopSonar();
        }
    }

    void ActivateTopSonar()
    {
        SpinAnim();
        if(!domainExpanded)
        {
            domainExpanded= true;
            Instantiate(AOESonar, transform.position, Quaternion.identity);
        }

        topCounter -= Time.deltaTime;

        if(topCounter < 0)
        {
            ExitTopSonar();
        }
    }

    void ExitTopSonar()
    {
        StopAnim();
        isSpinning = false;
        isReadyToSpin = false;
        domainExpanded = false;

        if (handState.handNotEmpty)
        {
            toolboxHelper.HopToEmptyBox();
        }
        else
        {
            ReturnToHand();
        }
        topCounter = topLifetime;
    }

    void ReturnToHand()
    {
        transform.parent = hand.transform;
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void TrackPositions()
    {
        if(trackedPositions.Count > 15) 
        {
            trackedPositions.RemoveAt(0);
        }
        else trackedPositions.Add(transform.position);
    }

    void ReleaseTop()
    {
       Vector3 direction = trackedPositions[^1] - trackedPositions[0];
        transform.parent= null;
        rb.isKinematic = false;
        rb.AddForce(direction * force);
        print("top released");

    }

    void SpinAnim()
    {
        transform.up = Vector3.up;
        anim.SetBool("Spin", true);
        sonarDust.SetActive(true);
    }

    void StopAnim()
    {
        anim.SetBool("Spin", false);
        sonarDust.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //on entry into pull collider, release top and add force
        if (other.CompareTag("PullTrigger"))
        {
            
                isReadyToSpin = true;
                ReleaseTop();

        }

        if (other.CompareTag("ToyBox"))
        {
            rb.isKinematic=true;
            ExitTopSonar();
        }

       

    }

    private void OnTriggerStay(Collider other)
    {
        //while in hand, track position
      
        if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {
            hand = other.gameObject;
            handState = hand.GetComponent<HandAnimation>();

            ToyToolboxInteractionManager topToolboxHelper = GetComponent<ToyToolboxInteractionManager>();
            topToolboxHelper.ActivateColliders();
            rb.isKinematic = true;
            TrackPositions();
       
                
        }
     
        
    }

    

}
