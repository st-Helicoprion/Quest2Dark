using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubLimiter : MonoBehaviour
{
    public Transform stickPivotPoint, tubPivotPoint;
    public ConfigurableJoint tubJoint;
  
    // Update is called once per frame
    void Update()
    {
        tubPivotPoint.LookAt(stickPivotPoint);
       
    }

    void LockTubMotion()
    {
        tubJoint.yMotion = ConfigurableJointMotion.Locked;
        
    }
}
