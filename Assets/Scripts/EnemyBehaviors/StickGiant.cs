using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.LowLevel;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(AudioSource))]
public class StickGiant : MonoBehaviour
{
    //monster behavior type : watchtower, creeper, trap-like, passive 

    public NavMeshAgent agent;
    public Transform target;
    public Vector3 idleTarget;
    public Animator anim;
    public bool refreshTarget, creeping, isNearPlayer, tracking;
    public float count, creepCount;
    public LineRenderer lineToPlayer;
    public AudioSource audioSource, warningAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("NewPlayerPrefab(Clone)").transform;
        lineToPlayer = GetComponent<LineRenderer>();

        WalkAnim();
     
    }

    // Update is called once per frame
    void Update()
    {
        if (!isNearPlayer&&!creeping)
        {
            DetectTargetReached();
            CreepToPlayer();
        }

        if(tracking) AlertAllMonsters();

        if(!tracking)CheckPlayerDistance(10);
       
    }

    void Wander()//monster walks randomly around map
    {
        if (audioSource.volume < 1)
            audioSource.volume += 0.1f;
        
        if (!audioSource.isPlaying && !tracking)
        {
            audioSource.PlayOneShot(AudioManager.instance.StickGiantAudioClips[0]);
        }
        else return;
        idleTarget = this.transform.position + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
        agent.speed = 1;
        agent.SetDestination(idleTarget);
        WalkAnim();

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
            if (!warningAudioSource.isPlaying && !tracking)
            {
                warningAudioSource.PlayOneShot(AudioManager.instance.StickGiantAudioClips[0]);
            }
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
        WalkAnim();

        yield return new WaitForSeconds(5);

        RefreshIdleTarget();
        creeping = false;
    }

    void AlertAllMonsters()
    {   
        agent.speed = 0;
        IdleAnim();

        lineToPlayer.enabled = true;
        lineToPlayer.SetPosition(0, this.transform.position + new Vector3(0, 15, 0));
        lineToPlayer.SetPosition(1, target.position);

        //catches all agents and gets all of them to track player
        NavMeshAgent[] allAgents = FindObjectsOfType<NavMeshAgent>();
        for (int i = 0; i < allAgents.Length; i++)
        {
            if (!allAgents[i].gameObject.CompareTag("Giant"))
            {
                if (allAgents[i].gameObject.CompareTag("Finger"))
                {
                    allAgents[i].GetComponent<FingerMonster>().ForcedSonarDetected();
                    allAgents[i].GetComponent<FingerMonster>().isControlled= true;
                }

            }

            }

        if (audioSource.volume < 1)
            audioSource.volume += 0.1f;
        else audioSource.volume = 1;

            
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(AudioManager.instance.StickGiantAudioClips[1]);
        }

        CheckForRemoveTracker();
        
    }

    void CheckForRemoveTracker()
    {
        CheckPlayerDistance(40);
        if(!isNearPlayer)
        {
            RemoveTracker();
        }
    }

    void RemoveTracker()
    {
        StartCoroutine(AudioVolumeFade());

        tracking = false;
        lineToPlayer.enabled = false;
        agent.speed = 1;
        NavMeshAgent[] allAgents = FindObjectsOfType<NavMeshAgent>();

        for (int i = 0; i < allAgents.Length; i++)
        {
            if (allAgents[i].gameObject.CompareTag("Finger"))
            {
                allAgents[i].GetComponent<FingerMonster>().FreezeMonster();
                allAgents[i].GetComponent<FingerMonster>().isControlled = false;
            }

        }
    }

    IEnumerator AudioVolumeFade()
    {
        while(audioSource.volume>0)
        audioSource.volume -= 0.1f;
        yield return null;
        audioSource.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sonar"))
        {
            agent.speed = 0;
            if (!audioSource.isPlaying&&!tracking)
            {
                audioSource.PlayOneShot(AudioManager.instance.StickGiantAudioClips[0]);
            }
        }
    }

    #region Animator
    void IdleAnim()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", true);
    }

    void WalkAnim()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Walk", true);
    }
    #endregion
}
