using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalSonarBehavior : MonoBehaviour
{
    public Transform playerSonar;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        playerSonar = GameObject.Find("PlayerSonar").transform;
    }

    private void Update()
    {
        IncreaseInternalSonarHeight();
    }

    void IncreaseInternalSonarHeight()
    {
        if (transform.localPosition.y < playerSonar.localPosition.y)
        {
            transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);

        }
        else Destroy(this.gameObject);

    }


}
