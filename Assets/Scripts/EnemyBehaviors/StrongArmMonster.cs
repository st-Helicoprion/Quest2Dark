using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class StrongArmMonster : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 idleTarget;
    public Material chaseMaterial, idleMaterial;
    public AudioSource audioSource;
    public bool refreshTarget, isNearPlayer, creeping, chasing;
    public float creepCount, huntRadius;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
       

        Wander();
    }

    // Update is called once per frame
    void Update()
    {
        DetectTargetReached();
    }

    void Wander()//monster walks randomly around map
    {
       /* SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = idleMaterial;
        renderer.materials = enemyMat;*/
        int rand1 = UnityEngine.Random.Range(-8, -3), rand2 = UnityEngine.Random.Range(3, 8);
        idleTarget = this.transform.position + new Vector3(rand1, 0, rand2);
        agent.speed = 1;
        agent.SetDestination(idleTarget);
       
    }

    public void FreezeMonster()
    {
        //StartCoroutine(AudioVolumeFade());

      /*  SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = idleMaterial;
        renderer.materials = enemyMat;*/
        agent.speed = 0;
       /* IdleAnim();

        attackHitbox.SetActive(false);
        isChasing = false;*/
    }

    void DetectTargetReached() //monster stops when reaching random target
    {
        if (Mathf.Abs(agent.transform.position.x - idleTarget.x) < 2)
        {
            FreezeMonster();
        }
        else return;
        /*if (Mathf.Abs(agent.transform.position.x - idleTarget.x) < 2 && !isChasing)
        {
            FreezeMonster();
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
        creeping = false;
    }

    void CheckPlayerDistance()
    {
        float distToPlayer = Vector3.Distance(transform.position, target.position);
        if (distToPlayer < 20)
        {
            isNearPlayer = true;
        }
        else isNearPlayer = false;
    }
}
