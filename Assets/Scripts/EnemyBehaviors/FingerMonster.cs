using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
    public bool isControlled, isChasing, charged;
    public GameObject attackHitbox;
    public Animator anim;
    public AudioSource audioSource;
    public float triggeredCount, triggeredLifetime, wakeDelay;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attackHitbox = this.transform.Find("AttackHitbox").gameObject;

        Wander();
    }

    private void Update()
    {
        DetectTargetReached();

        if(isControlled)
        {
            ForcedSonarDetected();
        }

        if(charged)
        {
            triggeredCount -= Time.deltaTime;
            ForcedSonarDetected();

            if(triggeredCount < 0&&!isControlled)
            {
                charged= false;
                FreezeMonster();
            }
        }
    }

    public void DetectedFeedback()
    {
        audioSource.volume = 1;
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = chaseMaterial;
        renderer.materials = enemyMat;
        
        if(!isChasing)
        {
            WakeAndRunAnim();
            Debug.Log("sonar detected");
            attackHitbox.SetActive(true);
            isChasing = true;
        }
        
    }

    public void SonarDetected(Collider other)//monster starts to chase source of noise
    {
        DetectedFeedback();
        wakeDelay -= Time.deltaTime;
        if(wakeDelay < 0)
        {
            agent.speed = 4;
        }
        agent.SetDestination(other.transform.position);
        //agent.transform.LookAt(other.transform.position);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(AudioManager.instance.FingerMonsterAudioClips[0]);
        }
    }

    public void ForcedSonarDetected()//monster starts to chase player
    {
        DetectedFeedback();
        wakeDelay -= Time.deltaTime;
        if (wakeDelay < 0)
        {
            agent.speed = 4;
        }
        agent.SetDestination(target.position);
        //agent.transform.LookAt(target.position);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(AudioManager.instance.FingerMonsterAudioClips[0]);
        }
    }

    public void FreezeMonster()
    {
        //StartCoroutine(AudioVolumeFade());

        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] enemyMat = renderer.materials;
        enemyMat[1] = idleMaterial;
        renderer.materials = enemyMat;
        agent.speed = 0;
        IdleAnim();
        wakeDelay = 3.5f;
        attackHitbox.SetActive(false);
        isChasing = false;
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
        WakeAndRunAnim();
        Debug.Log("Stone summoned and roaming");
    }

    void DetectTargetReached() //monster stops when reaching random target
    {
        if (Mathf.Abs(agent.transform.position.x - idleTarget.x) < 2&&!isChasing)
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

    IEnumerator KnockbackCoroutine(Collision collision)
    {
        for(int i = 0;i < 10; i++)
        {
            transform.localPosition -= transform.forward;
            yield return null;
        }
      
    }
    IEnumerator AudioVolumeFade()
    {
        while (audioSource.volume > 0)
            audioSource.volume -= 0.1f;
        yield return null;
        audioSource.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {

            StartCoroutine(KnockbackCoroutine(collision));

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GunSonar") || other.CompareTag("PlaneSonar"))
        {
            charged = true;
            triggeredCount = triggeredLifetime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sonar"))
        {
            ForcedSonarDetected();
        }

      
        if (other.CompareTag("TopSonar"))
        {
            SonarDetected(other);

            if(other == null)
            {
                FreezeMonster();
            }
        }

        

    }

    private void OnTriggerExit(Collider other)
    {
        //monster leaves sonar and stops moving
        if(other.CompareTag("Sonar")&&!isControlled)
        {
            FreezeMonster();
        }
        if(other.CompareTag("TopSonar")&&!isControlled||
           other.CompareTag("GunSonar") && !isControlled||
           other.CompareTag("PlaneSonar") && !isControlled)
        {
            FreezeMonster();
        }
    }

    #region Animator
    void IdleAnim()
    {
        anim.SetBool("Idle", true);
    }

    void WakeAndRunAnim()
    {
        anim.SetBool("Idle", false);
        anim.SetTrigger("Wake");
    }
    #endregion

}
