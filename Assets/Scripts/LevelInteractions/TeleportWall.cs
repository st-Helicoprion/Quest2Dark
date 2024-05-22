using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = spawnPoint.position+new Vector3(0,2,0);
        }
    }
}
