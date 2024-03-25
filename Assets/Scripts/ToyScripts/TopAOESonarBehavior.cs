using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopAOESonarBehavior : MonoBehaviour
{
    public int maxSonarSize;
    public float topSonarCount;
    public GameObject topSonarPrefab;
    public Transform top;

    // Start is called before the first frame update
    void Start()
    {
        top = FindFirstObjectByType<CustomTopManager>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = top.localPosition;

        if (CustomTopManager.isSpinning)
        {
            ExpandToEdge();
            ActivateTopSonar();

        }
        else
        {
            ShrinkSonar();
        }
    }

    void ActivateTopSonar()
    {

        topSonarCount += Time.deltaTime;

        if (topSonarCount > 1)
        {
            Instantiate(topSonarPrefab, transform.position, Quaternion.identity);
            Debug.Log("sonar released");
            topSonarCount = 0;
        }
    }

    void ExpandToEdge()
    {
        if (transform.localScale.x < maxSonarSize)
        {
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * 7;

        }
        
    }

    void ShrinkSonar()
    {
        if (transform.localScale.x > 0.1f)
        {
            transform.localScale -= new Vector3(1,1,1) * 5;

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
