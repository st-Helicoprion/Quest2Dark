using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSonarBehavior : MonoBehaviour
{
    private void Update()
    {
        if (transform.localPosition.y < -5)
        {
            transform.localPosition += new Vector3(0, 15 * Time.deltaTime, 0);
        }
        else Destroy(this.gameObject);
      
    }

  
}
