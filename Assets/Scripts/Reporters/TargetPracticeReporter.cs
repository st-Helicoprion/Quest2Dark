using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPracticeReporter : MonoBehaviour
{
    public TutorialSonarReporter tutSonar;
    public Renderer sonarSkin; public Collider sonarColl;
    public Renderer[] boxSkin; public Collider boxColl;

   public void ShowBox()
    {
        boxColl.enabled= true;
        foreach(Renderer r in boxSkin) 
        {
            r.enabled = true;
        }
    }

    public void ShowSonar()
    {
        tutSonar.enabled = true;
        sonarColl.enabled= true;
        sonarSkin.enabled= true;
    }
}
