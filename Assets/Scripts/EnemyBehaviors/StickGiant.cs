using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.LowLevel;
using UnityEngine.UIElements.Experimental;

public class StickGiant : MonoBehaviour
{
    //monster behavior type : watchtower, creeper, trap-like, passive 

    public NavMeshAgent agent;
    public Transform target;
    public Vector3 idleTarget;
    public GameObject attackHitbox;
    public Animator anim;
    public bool refreshTarget, creeping, isNearPlayer, tracking;
    public float count, creepCount;
    public LineRenderer lineToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attackHitbox = this.transform.Find("AttackHitbox").gameObject;
        lineToPlayer = GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isNearPlayer&&!creeping)
        {
            DetectTargetReached();
            CreepToPlayer();
        }

        if (tracking)
        {
            AlertAllMonsters();
        }
        }

    void Wander()//monster walks randomly around map
    {

        idleTarget = this.transform.position + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
        agent.speed = 1;
        agent.SetDestination(idleTarget);

    }
    void DetectTargetReached()//used for continuous wandering behavior
    {
        if (Mathf.Abs(agent.transform.position.x - idleTarget.x) < 1)
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
        CheckPlayerDistance(20);
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
    void CheckPlayerDistance(int minDistance)
    {
        float distToPlayer = Vector3.Distance(transform.position, target.position);
        if (distToPlayer < minDistance)
        {
            isNearPlayer = true;
            agent.speed = 0;
        }
        else isNearPlayer = false;
    }

    void CreepToPlayer()//to keep player from standing still
    {

        if (target.GetComponent<CharacterController>().velocity == Vector3.zero && !isNearPlayer)
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
        creeping = false;
    }

    void AlertAllMonsters()
    {
            agent.speed = 0;

            lineToPlayer.enabled = true;
            lineToPlayer.SetPosition(0, this.transform.position + new Vector3(0, 15, 0));
            lineToPlayer.SetPosition(1, target.position);

            //catches all agents and get all of them to track player
            NavMeshAgent[] allAgents = FindObjectsOfType<NavMeshAgent>();
            Debug.Log("all enemies alerted");
            for (int i = 0; i < allAgents.Length; i++)
            {
                if (!allAgents[i].gameObject.CompareTag("Giant"))
                {
                    if (allAgents[i].gameObject.CompareTag("Finger"))
                    {
                         allAgents[i].gameObject.GetComponent<FingerMonster>().SonarDetected(target.Find("PlayerSonar").GetComponent<Collider>());
                    }
                    
                }

            }

            CheckForRemoveTracker();
        
        
    }

    void CheckForRemoveTracker()
    {
        CheckPlayerDistance(40);
        if(!isNearPlayer)
        {
            Debug.Log("target lost");
            RemoveTracker();
        }
    }

    void RemoveTracker()
    {
        tracking=false;
        lineToPlayer.enabled = false;
        agent.speed = 1;
        NavMeshAgent[] allAgents = FindObjectsOfType<NavMeshAgent>();
        Debug.Log("all enemies alerted");
        for (int i = 0; i < allAgents.Length; i++)
        {
            if (allAgents[i].gameObject.CompareTag("Finger")&&allAgents[i].gameObject.GetComponent<FingerMonster>().chasing)
            {
                allAgents[i].GetComponent<FingerMonster>().chasing = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sonar"))
        {
            agent.speed = 0;
        }
    }
}
