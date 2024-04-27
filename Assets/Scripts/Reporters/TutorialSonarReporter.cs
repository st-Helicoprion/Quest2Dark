using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSonarReporter : MonoBehaviour
{
    public Transform playerSonar;
    public bool primed;
    // Start is called before the first frame update
    void Start()
    {
        playerSonar = GameObject.Find("PlayerSonar").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerSonar!= null)
        {
            if(playerSonar.position.y>transform.position.y)
            {
                TutorialsManager.instance.cicadaGoal = true;
                Destroy(gameObject);
            }
        }

        if(primed)
        {
            if(CustomTopManager.isSpinning)
            {
                TutorialsManager.instance.topGoal = true;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SpinningTop"))
        {
            TutorialsManager.instance.topGoal = true;
            primed = true;
        }
    }
}
