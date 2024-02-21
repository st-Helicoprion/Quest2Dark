using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowVisuals : MonoBehaviour
{
    public Transform mainCamera;
    public SpriteRenderer marker;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        marker = GameObject.Find("HitboxMarker").GetComponent<SpriteRenderer>();

        marker.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("PullTrigger"))
                {
                    marker.enabled = true;
                    marker.transform.position = hit.point;

                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        marker.enabled = false;
    }
}
