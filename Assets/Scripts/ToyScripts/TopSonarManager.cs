using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSonarManager : MonoBehaviour
{
    public Transform top;
    public GameObject effectAreaSonar, sonarDust;
    public static bool isSpinTop, isInBox, isReadyToSpin, isInHand, isNotFound, domainExpanded;
    public int maxSonarSize;
    public Animator anim;
    public EquipVisibilityManager visibilityManager;

    // Start is called before the first frame update
    void Start()
    {
        top = this.transform;
        isSpinTop= false;
        isNotFound = true;
        anim = transform.GetChild(0).GetComponent<Animator>();
        visibilityManager=GetComponent<EquipVisibilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpinTop)
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
        else return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            visibilityManager.isHideable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("TopThreshold"))
        {
            Debug.Log("top released");
            isReadyToSpin = true;
            isSpinTop= false;
            isInBox= false;
            isInHand= false;
            visibilityManager.isHideable= false;

            PlayerStateManager.CheckPlayerState();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("LeftHand")||other.CompareTag("RightHand"))
        {
            isReadyToSpin = false;
            isSpinTop = false;
            isInBox = false;
            isInHand = true;
            isNotFound = false;
            visibilityManager.isHideable = false;
            StopAnim();
            Debug.Log("top retracted");

            PlayerStateManager.CheckPlayerState();
        }

        if(other.CompareTag("ToySpawn"))
        {
            isReadyToSpin = false;
            isSpinTop = false;
            isInBox = true;
            isInHand = false;
            if (ToolboxManager.instance.isVisible)
                visibilityManager.isHideable = true;
            Debug.Log("top retracted");

            PlayerStateManager.CheckPlayerState();
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
