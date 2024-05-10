using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubLimiter : MonoBehaviour
{
    public Transform stickPivotPoint, tubPivotPoint;
    
  
    // Update is called once per frame
    void Update()
    {
        tubPivotPoint.LookAt(stickPivotPoint);
       
    }

}
