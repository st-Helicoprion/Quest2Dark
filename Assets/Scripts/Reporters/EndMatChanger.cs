using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMatChanger : MonoBehaviour
{
    public Material[] endMat;
    public Renderer[] rend;
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("EndShifter"))
        {
            foreach(Renderer r in rend)
            {
                r.materials = endMat;
            }
            
        }
        else return;
    }  
}
