using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndReporter : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

        }
    }
}
