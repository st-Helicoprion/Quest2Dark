using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopInternalSonarBehavior : MonoBehaviour
{
    public Transform topSonar;
    private void Start()
    {
        topSonar = GameObject.FindWithTag("TopSonar").transform.parent;
    }
    // Update is called once per frame
    void Update()
    {
        ClimbToTop();
    }

    void ClimbToTop()
    {
        transform.localPosition = new Vector3(topSonar.localPosition.x, transform.localPosition.y, topSonar.localPosition.z);
        if (transform.localPosition.y <topSonar.localPosition.y)
        {
            transform.localPosition += new Vector3(0, 15 * Time.deltaTime, 0);
        }
        else Destroy(this.gameObject);
       
    }


}

