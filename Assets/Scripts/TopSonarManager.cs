using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSonarManager : MonoBehaviour
{
    public Transform top;
    public GameObject topSonarPrefab, pullTrigger, topThreshold;
    public float topSonarCount, topSonarCountToShrink;
    public static bool isSpinTop;
    public int maxSonarSize;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        top = this.transform;
        topThreshold = GameObject.FindWithTag("TopThreshold");
        topThreshold.GetComponent<BoxCollider>().enabled = false;
        pullTrigger = FindObjectOfType<PullColliderBehavior>().gameObject;
        pullTrigger.GetComponent<BoxCollider>().enabled = false;
        isSpinTop= false;
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpinTop)
        {
            
            ActivateTopSonar();

        }
     

    }

   
    void CheckTopSonarHitMaxSize()
    {
        if (top.localScale.x > maxSonarSize)
        {
            topSonarCountToShrink += Time.deltaTime;
            if (topSonarCountToShrink > 5)
            {
                ShrinkTopSonar();
                topSonarCount = 0;
            }
        }
        else return;
    }

    void ShrinkTopSonar()
    {
        if(top.localScale.x>0.2f)
        {
            top.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime)*10;
        }
    }

    void ActivateTopSonar()
    {
        topSonarCount += Time.deltaTime;
        SpinAnim();

        if (topSonarCount>1)
        {
            Instantiate(topSonarPrefab, transform.position, Quaternion.identity);
            Debug.Log("sonar released");
            topSonarCount = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("TopThreshold") && !isSpinTop)
        {
            Debug.Log("top released");
            topThreshold.GetComponent<BoxCollider>().enabled = false;
            pullTrigger.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isSpinTop = false;
            topThreshold.GetComponent<BoxCollider>().enabled = true;
            pullTrigger.GetComponent<BoxCollider>().enabled = false;
            Debug.Log("top retracted");
            StopAnim();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isSpinTop = false;
            pullTrigger.GetComponent<BoxCollider>().enabled = false;
            StopAnim();
        }
    }

    void SpinAnim()
    {
        anim.SetBool("Spin", true);
    }

    void StopAnim()
    {
        anim.SetBool("Spin", false);
    }
}
