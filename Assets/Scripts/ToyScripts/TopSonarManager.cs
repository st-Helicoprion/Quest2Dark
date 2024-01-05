using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSonarManager : MonoBehaviour
{
    public Transform top;
    public GameObject effectAreaSonar, sonarDust;
    public static bool isSpinning, isInBox, isReadyToSpin, isInHand, domainExpanded;
    public Animator anim;
    public EquipVisibilityManager visibilityManager;
    public float topLifetime;
    public ToyToolboxInteractionManager toolboxInteractionHelper;

    // Start is called before the first frame update
    void Start()
    {
        top = this.transform;
        isSpinning= false;
        anim = transform.GetChild(0).GetComponent<Animator>();
        visibilityManager=GetComponent<EquipVisibilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpinning)
        {
            ActivateTopSonar();
        }
    }

    void ActivateTopSonar()
    {
        SpinAnim();

        if (!domainExpanded)
        {
            domainExpanded = true;
            Instantiate(effectAreaSonar, transform.position, Quaternion.identity);

        }

        topLifetime -= Time.deltaTime;

        if (topLifetime<0)
        {
            
        }
    }

    void ReturnToToolbox()
    {
        isSpinning = false;
        StopAnim();

                visibilityManager.isHideable = true;
                toolboxInteractionHelper.HopToEmptyBox();
                top.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("top returned");

         
                topLifetime = 5;
                domainExpanded = false;
            
        

       

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("LeftHand")|| other.CompareTag("RightHand"))
        {
            isInHand = true;
            isInBox = false;
            visibilityManager.isHideable = false;

            top.GetComponent<Rigidbody>().isKinematic = false;
            transform.parent = null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("TopThreshold"))
        {
            Debug.Log("top released");
            isReadyToSpin = true;
            isSpinning= false;
            isInBox= false;
            isInHand= false;
            domainExpanded=false;
            visibilityManager.isHideable= false;

          
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if(other.CompareTag("ToyBox"))
        {
            isReadyToSpin = false;
            isSpinning = false;
            isInBox = true;
            isInHand = false;
            domainExpanded = false;
            topLifetime = 5;
            if (ToolboxManager.instance.isVisible)
                visibilityManager.isHideable = true;
            Debug.Log("top retracted");
        }
        else if(other.CompareTag("LeftHand")||
            other.CompareTag("RightHand"))
        {
            
            isReadyToSpin = false;
            isSpinning = false;
         
            visibilityManager.isHideable = false;
            domainExpanded= false;
            topLifetime = 5;

            top.GetComponent<Rigidbody>().isKinematic = false;
            top.parent = null;

            StopAnim();
            Debug.Log("top retracted");
        }
    }

    void SpinAnim()
    {
        top.up = Vector3.up;
        anim.SetBool("Spin", true);
        sonarDust.SetActive(true);
    }

    void StopAnim()
    {
        anim.SetBool("Spin", false);
        sonarDust.SetActive(false);
    }
}
