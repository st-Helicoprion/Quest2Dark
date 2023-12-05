using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundToyPusher : MonoBehaviour
{
    public GameObject pushPedestal;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<KeyItemReporter>(out _))
        {
            Instantiate(pushPedestal, collision.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        }
    }
}
