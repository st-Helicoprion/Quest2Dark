using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopAOESonarBehavior : MonoBehaviour
{
    public int maxSonarHeight;
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
        if (CustomTopManager.isSpinning)
        {
            ClimbToTop();
            ActivateTopSonar();

        }
        else
        {
            LowerSonar();
        }
    }

    void ActivateTopSonar()
    {

        topSonarCount += Time.deltaTime;

        if (topSonarCount > 1)
        {
            Instantiate(topSonarPrefab, this.transform.parent);
            Debug.Log("sonar released");
            topSonarCount = 0;
        }
    }

    void ClimbToTop()
    {
        if (transform.localPosition.y < maxSonarHeight)
        {
            transform.localPosition += new Vector3(0, 18 * Time.deltaTime, 0);

        }
        
    }

    void LowerSonar()
    {
        if (transform.localPosition.y > -60)
        {
            transform.localPosition -= new Vector3(0, 120 * Time.deltaTime, 0);

        }
        
        if(transform.localPosition.y<-60)
        {
            Destroy(this.gameObject);
        }
    }
}
