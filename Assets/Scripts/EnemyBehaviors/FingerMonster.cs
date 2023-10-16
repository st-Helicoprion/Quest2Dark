using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FingerMonster : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 idleTarget;
    public Material chaseMaterial, idleMaterial;
    public bool refreshTarget, chasing, creeping;
    public float count, creepCount;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Wander();
    }

    private void Update()
    {
        if (!chasing&&!creeping)
        {
            DetectTargetReached();
            RefreshIdleTarget();
            CreepToPlayer();
        }

        if(PlayerKiller.killPlayer)
        {
            Wander();
        }

    }

    void PlayerDetected()//monster starts to chase player
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = chaseMaterial;
        renderer.materials = enemyMat;
        agent.speed = 3;
        agent.SetDestination(target.position);
        agent.transform.LookAt(target.position);
        Debug.Log("player detected");
    }

    void Wander()//monster walks randomly around map
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = idleMaterial;
        renderer.materials = enemyMat;
        idleTarget = this.transform.position + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
        agent.speed = 2;
        agent.SetDestination(idleTarget);
        
    }

    void DetectTargetReached()
    {
        if(Mathf.Abs(agent.transform.position.x-idleTarget.x)<1)
        {
            refreshTarget = true;
            RefreshIdleTarget();
        }

        count += Time.deltaTime;
        if (count > 5)
        {
            refreshTarget = true;
            RefreshIdleTarget();
            count = 0;
        }
        else return;
        
    }

    void RefreshIdleTarget()
    {

        if (refreshTarget == true)
        {
            Wander();
            refreshTarget = false;
            Debug.Log("idleTarget refreshed");
        }
        else return;
    }

    void CreepToPlayer()//to keep player from standing still
    {
        if (target.GetComponent<CharacterController>().velocity == Vector3.zero)
        {
            if (!creeping)
                creepCount += Time.deltaTime;

        }
        else return;
        if (creepCount > 10)
        {
            StartCoroutine(CreepCoroutine());
            creepCount = 0;
        }
    }
    IEnumerator CreepCoroutine()
    {
        Debug.Log("player not moving");
        agent.SetDestination(target.position);
        creeping = true;

        yield return new WaitForSeconds(5);

        RefreshIdleTarget();
        creeping= false;
    }

    private void OnTriggerStay(Collider other)
    {
        //monster enters inside sonar
        if(other.CompareTag("Sonar"))
        {
            chasing = true;//stops monster from wandering
            PlayerDetected();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //monster leaves sonar
        if (other.CompareTag("Sonar"))
        {
            chasing= false;
            Wander();
        }
    }
}
