using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowVisuals : MonoBehaviour
{
    public Transform mainCamera;
    public GameObject marker;
    public string markerName;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        marker = GameObject.Find(markerName);

        if(transform.CompareTag("SpinningTop"))
        {
            marker.SetActive(false);
        }
        else if(transform.CompareTag("Hopter"))
        {
            marker.transform.GetChild(0).gameObject.SetActive(false);
        }


    }

    private void Update()
    {
        if(transform.CompareTag("SpinningTop"))
        {
            if(CustomTopManager.isReadyToSpin)
            {
                int layer = 10;
                int layerMask = 1 << layer;
                if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
                {

                    if (hit.transform.CompareTag("PullTrigger"))
                    {
                        marker.SetActive(true);
                        marker.transform.position = hit.point - new Vector3(0, .2f, 0);

                    }

                }
            }
            else if(CustomTopManager.isSpinning)
            {
                marker.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            int layer = 10;
            int layerMask = 1 << layer;
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.CompareTag("PullTrigger")&&transform.CompareTag("Hopter"))
                {
                    marker.transform.GetChild(0).gameObject.SetActive(true);
                    marker.transform.position = hit.point+ new Vector3(0, .1f, 0);

                }

                if (hit.transform.CompareTag("PullTrigger") && transform.CompareTag("SpinningTop")&&!CustomTopManager.isSpinning)
                {
                    marker.SetActive(true);
                    marker.transform.position = hit.point-new Vector3(0,.1f,0);

                }

            }
        }

        if(other.CompareTag("ToyBox"))
        {
            if(transform.CompareTag("SpinningTop"))
            {
                marker.SetActive(false);
            }

            if (transform.CompareTag("Hopter"))
            {
                marker.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(transform.CompareTag("Hopter"))
        {
            marker.transform.GetChild(0).gameObject.SetActive(false);
        }


    }
}
