using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopInternalSonarBehavior : MonoBehaviour
{
    public int maxSonarSize;

    // Update is called once per frame
    void Update()
    {
        ExpandToEdge();
    }

    void ExpandToEdge()
    {
        if (transform.localScale.x < maxSonarSize)
        {
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 7;

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("TopSonar"))
        {
            Destroy(this.gameObject);
        }
    }

}

