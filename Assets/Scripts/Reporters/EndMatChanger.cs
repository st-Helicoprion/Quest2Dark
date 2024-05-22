using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMatChanger : MonoBehaviour
{
    public Material[] endMat;
    public Renderer[] rend;

    private void Update()
    {
        if (EndingManager.instance.changeMapMat)
        {
            foreach (Renderer r in rend)
            {
                r.materials = endMat;
            }

        }
    }
}
