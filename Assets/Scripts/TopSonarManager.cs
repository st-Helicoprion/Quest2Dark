using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSonarManager : MonoBehaviour
{
    public Transform top;
    public GameObject topSonarPrefab;
    public float topSonarCount;
    public static bool isSpinTop, isInBox, isReadyToSpin, isInHand, isNotFound;
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
        topSonarCount += Time.deltaTime;
        SpinAnim();

        if (topSonarCount>2)
        {
            Instantiate(topSonarPrefab, transform.position, Quaternion.identity);
            Debug.Log("sonar released");
            topSonarCount = 0;
        }
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
        }
    }

    void SpinAnim()
    {
        top.up = Vector3.up;
        anim.SetBool("Spin", true);
        top.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    void StopAnim()
    {
        anim.SetBool("Spin", false);
        top.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }
}
