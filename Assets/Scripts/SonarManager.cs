using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SonarManager : MonoBehaviour
{
    public Transform sonar;
    public static HitboxManager hitboxManager;
    public float maxSonarHeight, minSonarHeight;
    public GameObject internalSonar;
    public static bool isInternalSonarActive;

    // Start is called before the first frame update
    void Start()
    {
        maxSonarHeight = -20;
        minSonarHeight = -60;
        hitboxManager = GameObject.FindObjectOfType<HitboxManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        SonarHeightClamp();
        ActivateInternalSonar();
        DetectSonar();
       

    }

    void SonarHeightClamp()
    {
        
        maxSonarHeight = Mathf.Clamp(maxSonarHeight, -20, 20);
        Vector3 sonarHeight = sonar.localPosition;
        sonarHeight.y = Mathf.Clamp(sonarHeight.y, minSonarHeight, maxSonarHeight);
        sonar.localPosition = sonarHeight;
    }

    void DecreaseSonarHeight()
    {
        sonar.localPosition -= new Vector3(0,Time.deltaTime, 0);
       
    }

    void IncreaseSonarHeight()
    {
        sonar.localPosition += new Vector3(0, 0.1f , 0);
        
    }

    void ActivateInternalSonar()
    {
        if (sonar.localPosition.y > minSonarHeight)
        {
            if (isInternalSonarActive == false)
            {
                Instantiate(internalSonar, sonar.parent);
                isInternalSonarActive = true;

            }

        }
        else return;

    }
    
    void DetectSonar()
    {
        if (hitboxManager != null)
        {
            if (hitboxManager.isSonarUp == true)
            {
                IncreaseSonarHeight();

            }
            else DecreaseSonarHeight();

        }
        else return;
    }
}
