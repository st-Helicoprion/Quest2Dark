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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("RightHand")||other.CompareTag("LeftHand"))
        {
            RaycastHit hit;
            if(Physics.Raycast(mainCamera.position,mainCamera.forward, out hit))
            {
                if(hit.transform.CompareTag("PullTrigger"))
                {
                    marker.enabled =  true;
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
