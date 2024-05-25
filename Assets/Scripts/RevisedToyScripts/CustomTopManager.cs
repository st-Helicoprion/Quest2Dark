using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTopManager : MonoBehaviour
{
    public static bool isReadyToSpin, isSpinning, domainExpanded;
    public GameObject AOESonar, sonarDust, hand;
    public List<Vector3> trackedPositions = new();
    public float force;
    public Rigidbody rb;
    public Animator anim;
    public float topLifetime, topCounter;
    public ToyToolboxInteractionManager toolboxHelper;
    public HandAnimation handState;
    public AudioSource topAudioSource;

    public LineRenderer ropeAttach;
    public Renderer ropeVisual;

    // Start is called before the first frame update
    void Start()
    {
        topCounter = topLifetime;
        ropeVisual.enabled = false;

        isReadyToSpin = false;
        isSpinning = false;
        domainExpanded= false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!NewToolboxManager.isOpen)
        {
            if (isSpinning)
            {
                ActivateTopSonar();
                HideRope();

                if (handState != null)
                {
                    if (handState.grip)
                    {
                        ExitTopSonar();
                    }
                }

            }

            if (hand != null)
            {
                ropeAttach.SetPosition(0, hand.transform.position);
            }


            if (isReadyToSpin)
            {
                ropeAttach.SetPosition(1, transform.position);
                if (handState != null)
                {
                    if (handState.grip)
                    {
                        ExitTopSonar();
                    }
                }
            }
        }
        

        
    }

    void ActivateTopSonar()
    {
        SpinAnim();
        if (!topAudioSource.isPlaying)
        {
            topAudioSource.volume = 0.5f;
            topAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[2]);
        }
        if (!domainExpanded)
        {
            domainExpanded= true;
            Instantiate(AOESonar, new Vector3(transform.position.x, -60, transform.position.z), Quaternion.identity);
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
            HideRope();
            rb.isKinematic = true;
            toolboxHelper.HopToEmptyBox();
        }
        else
        {
            ReturnToHand();
            HideRope();
        }
        topCounter = topLifetime;
    }

    void ExitNoReturn()
    {
        StopAnim();
        isSpinning = false;
        isReadyToSpin = false;
        domainExpanded = false;
        topCounter = topLifetime;
        HideRope();
    }

    void ReturnToHand()
    {
        rb.isKinematic = true;
        toolboxHelper.StickToyToHand(handState, handState.handID);
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
       /* direction.x=Mathf.Clamp(direction.x, 0.7f, 10);
        direction.y = Mathf.Clamp(direction.y, 0.7f, 10);
        direction.z = Mathf.Clamp(direction.z, 0.7f, 10);*/
        transform.parent= null;
        rb.isKinematic = false;
       
        if(PlayerMoveFeedback.moving)
        {
            rb.AddForce(2 * force * direction);
        }else
        rb.AddForce(direction * force);
        topAudioSource.Stop();
        if (!topAudioSource.isPlaying)
        {
            topAudioSource.pitch = Random.Range(1, 1.3f);
            topAudioSource.PlayOneShot(AudioManager.instance.ToysSFX[0]);
        }
        print("top released");

    }

    void SpinAnim()
    {
        transform.up = Vector3.up;
        rb.isKinematic = false;
        rb.AddForce(10 * Vector3.down);
        anim.SetBool("Spin", true);
        sonarDust.SetActive(true);
    }

    void StopAnim()
    {
        anim.SetBool("Spin", false);
        sonarDust.SetActive(false);
    }

    void SummonRope()
    {
       
        ropeVisual.enabled = true;
    }

    void HideRope()
    {
        ropeVisual.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ToyBox"))
        {
            rb.isKinematic=true;
            ExitNoReturn();
        }

        //on entry into pull collider, release top and add force
        if (other.CompareTag("PullTrigger") && !isSpinning && toolboxHelper.isInHand)
        {
            isReadyToSpin = true;
            ReleaseTop();
            SummonRope();
        }
        
       

    }

    private void OnTriggerStay(Collider other)
    {
        //while in hand, track position
      
        if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {
            hand = other.gameObject;
            handState = hand.GetComponent<HandAnimation>();

            rb.isKinematic = true;
            TrackPositions();
       
                
        }
     
        
    }

    

}
