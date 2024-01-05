using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateReporter : MonoBehaviour
{
    public GameObject[] gates;
    public bool[] toggles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            for(int i =0; i < gates.Length;i++)
            {
                gates[i].SetActive(toggles[i]);
                
            }
        }
    }
}
