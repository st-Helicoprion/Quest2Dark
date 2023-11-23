using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullColliderBehavior : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("LeftHand")||other.CompareTag("RightHand"))
        {
            TopSonarManager.isSpinTop= true;
            this.GetComponent<BoxCollider>().enabled=false;
        }
    }
}
