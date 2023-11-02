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
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 5;

        }
        else
        {
            if(this.transform.GetComponent<SphereCollider>().center.y>-3)
            {
                this.transform.GetComponent<SphereCollider>().center += new Vector3(0, -1, 0);
            }
            else
            Destroy(this.gameObject);
            

        }
    }
}

