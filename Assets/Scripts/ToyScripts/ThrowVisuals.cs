using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowVisuals : MonoBehaviour
{
    public Transform mainCamera;
    public SpriteRenderer marker;
    public string markerName;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        marker = GameObject.Find(markerName).GetComponent<SpriteRenderer>();

        marker.enabled = false;
    }

    private void Update()
    {
        if(transform.CompareTag("SpinningTop"))
        {
            if(CustomTopManager.isReadyToSpin)
            {
                if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hit))
                {

                    if (hit.transform.CompareTag("PullTrigger"))
                    {
                        marker.enabled = true;
                        marker.transform.position = hit.point - new Vector3(0, .2f, 0);

                    }

                }
            }
            else if(CustomTopManager.isSpinning)
            {
                marker.enabled = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("PullTrigger")&&transform.CompareTag("Hopter"))
                {
                    marker.enabled = true;
                    marker.transform.position = hit.point+ new Vector3(0, .1f, 0);

                }

                if (hit.transform.CompareTag("PullTrigger") && transform.CompareTag("SpinningTop")&&!CustomTopManager.isSpinning)
                {
                    marker.enabled = true;
                    marker.transform.position = hit.point-new Vector3(0,.2f,0);

                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        marker.enabled = false;
    }
}
