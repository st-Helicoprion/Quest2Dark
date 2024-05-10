using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakStateMaterialChanger : MonoBehaviour
{
    public Material weakStateMat;
    public Material normalStateMat;
    public Renderer rend;
    public Renderer[] outlinedRend;
    public ParticleSystem pSystem;

    public bool outlined, pTrailed;

    // Update is called once per frame
    void Update()
    {
        if(WeakStateManager.instance.weakened)
        {
            SwitchToWeak();
        }
        else
        {
            SwitchToNormal();
        }
    }

    public void SwitchToWeak()
    {
        if(outlined)
        {
            for(int i = 0; i<outlinedRend.Length;i++)
            {
                outlinedRend[i].materials[1] = weakStateMat;
            }
        }
        else
            rend.material = weakStateMat;
       
       if(pTrailed)
        {
            ParticleSystemRenderer pTrailR = pSystem.GetComponent<ParticleSystemRenderer>();
            pTrailR.trailMaterial = weakStateMat;
        }
        
    }

    public void SwitchToNormal()
    {
        if (outlined)
        {
            for (int i = 0; i < outlinedRend.Length; i++)
            {
                outlinedRend[i].materials[1] = normalStateMat;
            }
        }
        else
            rend.material = normalStateMat;

        if (pTrailed)
        {
            ParticleSystemRenderer pTrailR = pSystem.GetComponent<ParticleSystemRenderer>();
            pTrailR.trailMaterial = normalStateMat;
        }
    }
}
