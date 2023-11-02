using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FingerMonster : MonoBehaviour
{
    //monster behavior type : trap-like, passive, mostly immobile

    public NavMeshAgent agent;
    public Transform target;
    public Vector3 idleTarget;
    public Material chaseMaterial, idleMaterial;
    public bool refreshTarget, chasing, creeping, isNearPlayer;
    public float count, creepCount;
    public GameObject attackHitbox;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attackHitbox = this.transform.Find("AttackHitbox").gameObject;

        Wander();
    }

    private void Update()
    {
        /*if (!chasing && !creeping)
        {
            DetectTargetReached();
            RefreshIdleTarget();
            CreepToPlayer();
        }*/ //behavior : creeping to player and then stopping

        DetectTargetReached();

        if (!chasing)
        {
            FreezeMonster();
        }

    }

    public void SonarDetected(Collider other)//monster starts to chase player
    {
        
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = chaseMaterial;
        renderer.materials = enemyMat;
        agent.speed = 3;
        agent.SetDestination(other.transform.position);
        agent.transform.LookAt(other.transform.position);
        RunAnim();
        Debug.Log("sonar detected");

        attackHitbox.SetActive(true);
    }

    void FreezeMonster()
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = idleMaterial;
        renderer.materials = enemyMat;
        agent.speed = 0;
        IdleAnim();

        attackHitbox.SetActive(false);
    }

    void Wander()//monster walks randomly around map
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = idleMaterial;
        renderer.materials = enemyMat;
        idleTarget = this.transform.position + new Vector3(UnityEngine.Random.Range(-8, 8), 0, UnityEngine.Random.Range(-8, 8));
        agent.speed = 1;
        agent.SetDestination(idleTarget);
        WalkAnim();
        
    }

    void DetectTargetReached() //monster stops when reaching random target
    {
        if (Mathf.Abs(agent.transform.position.x - idleTarget.x) < 2)
        {
            refreshTarget = true;
            FreezeMonster();
        }
        else return;

      /*  count += Time.deltaTime;
       
        if (count > 5)
        {
            refreshTarget = true;
            RefreshIdleTarget();
            count = 0;
        }
        else return;*/
        
    }

    void RefreshIdleTarget()
    {
        CheckPlayerDistance();
        if (refreshTarget == true)
        {
            refreshTarget = false;
            if (!isNearPlayer)
            {
                Debug.Log("idleTarget refreshed");
                Wander();
            }
            else return;
        }
        else return;
    }

    void CreepToPlayer()//to keep player from standing still
    {
        
        if (target.GetComponent<CharacterController>().velocity == Vector3.zero && !isNearPlayer)
        {
            if (!creeping)
                creepCount += Time.deltaTime;

        }
        else if (!chasing)
        {
            FreezeMonster();
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

    void CheckPlayerDistance()
    {
        float distToPlayer = Vector3.Distance(transform.position, target.position);
        if (distToPlayer <20)
        {
            isNearPlayer = true;
        }
        else isNearPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //monster enters inside sonar
        if(other.CompareTag("Sonar"))
        {
            chasing = true;
            SonarDetected(other);//starts to chase sonar source
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Sonar"))
        {
            chasing = true;
            SonarDetected(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //monster leaves sonar and stops moving
        if (other.CompareTag("Sonar"))
        {
            chasing= false;
            FreezeMonster();
        }
    }

    #region Animator
    void RunAnim()
    {
        anim.SetBool("Run", true);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
    }

    void IdleAnim()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", true);
    }

    void WalkAnim()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Walk", true);
        anim.SetBool("Idle", false);
    }
    #endregion
}
