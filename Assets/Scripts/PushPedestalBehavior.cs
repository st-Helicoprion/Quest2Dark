using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPedestalBehavior : MonoBehaviour
{
    public bool ascendIsDone, itemLost;
    public float ascendCount, descendCount;
  
    // Update is called once per frame
    void Update()
    {
        ascendCount += Time.deltaTime;

        if(ascendCount<3&&!ascendIsDone)
        {
            transform.Translate(Vector3.up*Time.deltaTime);

        }
        else ascendIsDone= true;
       

        if(itemLost)
        {
            descendCount += Time.deltaTime;
            if (descendCount < 3)
            {
                transform.Translate(-1 * Time.deltaTime * Vector3.up);
            }
            else Destroy(this.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.TryGetComponent<KeyItemReporter>(out _))
        {
            itemLost = true;
        }
    }
}
