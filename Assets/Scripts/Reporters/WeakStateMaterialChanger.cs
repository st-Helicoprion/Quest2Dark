using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakStateMaterialChanger : MonoBehaviour
{
    public Material weakStateMat;
    public Material normalStateMat;
    public Renderer rend;
    public Renderer[] outlinedRend;
    public Material[] weakOutlines, normalOutlines;
    public ParticleSystem pSystem;

    public bool outlined, pTrailed, standard;

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

        if (transform.CompareTag("RightHand") || transform.CompareTag("LeftHand")
           || transform.CompareTag("Steps") || transform.CompareTag("ToyBox"))
        {
            if (!ToyToolboxInteractionManager.itemTaken)
            {
                SwitchToWeak();
            }
            else if(WeakStateManager.instance.weakened)
            {
                SwitchToWeak();
            }
            else
            { 
                SwitchToNormal();
            }
        }
    }

    public void SwitchToWeak()
    {
        if(outlined)
        {
            
            foreach(Renderer r in outlinedRend)
            {
                r.materials = weakOutlines;
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
            
            foreach (Renderer r in outlinedRend)
            {
                r.materials = normalOutlines;
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
