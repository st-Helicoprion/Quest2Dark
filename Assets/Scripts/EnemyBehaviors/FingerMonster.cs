using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class FingerMonster : MonoBehaviour
{
    //monster behavior type : trap-like, passive, mostly immobile
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 idleTarget;
    public Material chaseMaterial, idleMaterial;
    //public bool creeping, isNearPlayer, refreshTarget;
    //public float count, creepCount;
    public bool isControlled;
    public GameObject attackHitbox;
    public Animator anim;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attackHitbox = this.transform.Find("AttackHitbox").gameObject;

        Wander();
    }

    private void Update()
    {
        DetectTargetReached();

        /*if (!chasing && !creeping)
        {
            DetectTargetReached();
            RefreshIdleTarget();
            CreepToPlayer();
        }*/ //behavior : creeping to player and then stopping

        if(isControlled)
        {
            ForcedSonarDetected();
        }

    }

    public void DetectedFeedback()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(AudioManager.instance.FingerMonsterAudioClips[0]);

        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = chaseMaterial;
        renderer.materials = enemyMat;

        RunAnim();
        Debug.Log("sonar detected");
        attackHitbox.SetActive(true);
    }

    public void SonarDetected(Collider other)//monster starts to chase source of noise
    {
        DetectedFeedback();
        agent.speed = 3;
        agent.SetDestination(other.transform.position);
        agent.transform.LookAt(other.transform.position);  
    }

    public void ForcedSonarDetected()//monster starts to chase player
    {
        DetectedFeedback();
        agent.speed = 3;
        agent.SetDestination(target.position);
        agent.transform.LookAt(target.position);
    }

    public void FreezeMonster()
    {
        audioSource.Stop();

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
        int rand1 = UnityEngine.Random.Range(-8, -3), rand2 = UnityEngine.Random.Range(3, 8);
        idleTarget = this.transform.position + new Vector3(rand1, 0, rand2);
        agent.speed = 1;
        agent.SetDestination(idleTarget);
        WalkAnim();
        Debug.Log("Stone summoned and roaming");
    }

    void DetectTargetReached() //monster stops when reaching random target
    {
        if (Mathf.Abs(agent.transform.position.x - idleTarget.x) < 2)
        {
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

    #region CreepingBehavior
    /* void RefreshIdleTarget()
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
     }*/

    /* void CreepToPlayer()//to keep player from standing still
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
     }*/
    /* IEnumerator CreepCoroutine()
     {
         Debug.Log("player not moving");
         agent.SetDestination(target.position);
         creeping = true;

         yield return new WaitForSeconds(5);

         RefreshIdleTarget();
         creeping= false;
     }*/

    /*void CheckPlayerDistance()
    {
        float distToPlayer = Vector3.Distance(transform.position, target.position);
        if (distToPlayer <20)
        {
            isNearPlayer = true;
        }
        else isNearPlayer = false;
    }*/
    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sonar"))
        {
            ForcedSonarDetected();
        }
        else if (other.CompareTag("TopSonar"))
        {
            SonarDetected(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //monster leaves sonar and stops moving
        if(other.CompareTag("Sonar")&&!isControlled)
        {
            FreezeMonster();
        }
        if(other.CompareTag("TopSonar")&&!isControlled)
        {
            FreezeMonster();
        }
    }

    #region Animator
    void RunAnim()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", true);
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
        anim.SetBool("Idle", false);
        anim.SetBool("Walk", true);
    }
    #endregion
}
