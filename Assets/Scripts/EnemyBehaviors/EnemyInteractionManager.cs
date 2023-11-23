using Obi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInteractionManager : MonoBehaviour
{
    public static bool killPlayer;
    public float giantCountToKill;
    public MeshRenderer rend;
    public StickGiant giantEnemy;

    public void Start()
    {
        CheckForRenderer();
        CheckEnemyType();
    }

    void CheckForRenderer()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            rend = GetComponent<MeshRenderer>();
            rend.enabled = false;
        }
        else return;
    }

    void CheckEnemyType()
    {
        if(transform.parent.CompareTag("Giant"))
        {
            giantEnemy = transform.parent.GetComponent<StickGiant>();
        }
    }

    #region GiantInteractions
    void GiantKillsPlayer(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            giantCountToKill += Time.deltaTime;
            if (giantCountToKill > 10)
            {
                other.tag = "Untagged";
                rend.enabled = true;
                killPlayer = true;
                giantCountToKill = 0;
            }
        }
        else return;
    }

    void GiantAndFingerInteraction(Collider fingerMonster)
    {
        if (transform.parent.CompareTag("Giant"))
        {
            if (fingerMonster.CompareTag("Finger"))
            {
                rend.enabled = true;
                StartCoroutine(GiantAndFingerCoroutine(fingerMonster));
            }
            else return;
        }
        else return;
    }

    IEnumerator GiantAndFingerCoroutine(Collider fingerMonster)
    {
        fingerMonster.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        yield return new WaitForSeconds(5);
        Destroy(fingerMonster.gameObject);
        GiantNoDangerSignal();
        
    }

    void GiantPlaceTracker(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           giantEnemy.tracking = true;

        }
    }

    void GiantNoDangerSignal()
    {
        if(transform.parent.CompareTag("Giant"))
        rend.enabled = false;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        //GiantAndFingerInteraction(other);

        if (!transform.parent.CompareTag("Giant"))
        {
            if (other.CompareTag("Player"))
            {
                killPlayer = true;
                other.tag= "Untagged";
                Debug.Log("player is dead");
                
            }

        }else GiantPlaceTracker(other);
       
    }


    private void OnTriggerStay(Collider other)
    {
        GiantKillsPlayer(other);
    }

    private void OnTriggerExit(Collider other)
    {
        giantCountToKill = 0;
        //StopCoroutine(GiantAndFingerCoroutine(other));
        GiantNoDangerSignal();
    }
}
