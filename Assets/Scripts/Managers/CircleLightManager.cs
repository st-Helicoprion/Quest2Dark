using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLightManager : MonoBehaviour
{
    public Color[] sonarColor;
    public Material sonarMat, sonarDustMat;

    private void Start()
    {
        ChangeLightColor(0);
    }
    public void ChangeLightColor(int itemID)
    {
        sonarMat.color = sonarColor[itemID];
        sonarDustMat.color = sonarColor[itemID];
    }
}
