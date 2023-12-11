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
            if(keyItem.itemID==3&&TopSonarManager.isNotFound)
            {
                Instantiate(pushPedestal, collision.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
            }

            if(keyItem.itemID!=3)
            {
                Instantiate(pushPedestal, collision.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
            }
        }
    }
}
