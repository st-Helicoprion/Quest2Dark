using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitReporter : MonoBehaviour
{
    public CicadaHitboxManager hitboxManager;
    public int hitboxID;

    private void Start()
    {
        hitboxManager = GameObject.FindObjectOfType<CicadaHitboxManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buzz"))
        {
            hitboxManager.hitboxIDList.Add(hitboxID);
        }
        else return;
    }

}
