using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundToyPusher : MonoBehaviour
{
    public GameObject pushPedestal;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<KeyItemReporter>(out KeyItemReporter keyItem))
        {
           

            if(keyItem.itemID!=3)
            {
                
                Instantiate(pushPedestal, collision.transform.transform.position + new Vector3(0, -2.5f, 0), Quaternion.identity);
            }
        }
    }
}
