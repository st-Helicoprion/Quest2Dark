using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class RunnerMonster : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target, sonar;
    public AudioSource audioSource;
    public static bool warn, canShowLanes, deployed, switching;
    public float huntRadius;

    public List<Transform> runningPoints1, runningPoints2, runningPoints3;
    public static int targetPointID, laneID;
    public static LineRenderer attackLine1, attackLine2, attackLine3;
    public Material lineMatBlank, lineMatActive;
    public Renderer sonarSkin;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        sonarSkin = sonar.GetComponent<Renderer>();

        attackLine1 = GameObject.Find("AttackLine1").GetComponent<LineRenderer>();
        attackLine2 = GameObject.Find("AttackLine2").GetComponent<LineRenderer>();
        attackLine3 = GameObject.Find("AttackLine3").GetComponent<LineRenderer>();

        InitRunningPoints();
        BindLanePoints();
        ShuffleLane();
        HideAttackLane();

        deployed = true;

        
    }

    // Update is called once per frame
    void Update()
    {
        SetRunSequence();

        if(Vector3.Distance(target.position, transform.position)<huntRadius)
        {
            if(Vector3.Dot(transform.forward, target.position-transform.position)>0)
            {
                if(canShowLanes)
                {
                    if (laneID == FishTrackSensor.playerLaneID.x ||
                       laneID == FishTrackSensor.playerLaneID.y ||
                       laneID == FishTrackSensor.playerLaneID.z)
                    {
                        if (!warn)
                        {
                            warn = true;
                            sonarSkin.enabled = true;
                            StartCoroutine(ReleaseWarningSonar());

                        }
                    }
                    else return;
                    
                }
              
            }
            else
            {
                warn = false;
                StopCoroutine(ReleaseWarningSonar());
                sonarSkin.enabled = false;
                sonar.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                canShowLanes = false;
                HideAttackLane();

            }
        
        }
        else
        {
            warn = false;
            StopCoroutine(ReleaseWarningSonar());
            sonarSkin.enabled = false;
            sonar.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            canShowLanes = false;
            HideAttackLane();
        }
        


    }

    
    void SetRunSequence()
    {
        if (laneID == 1)
        {
            if (Mathf.Abs(transform.position.x - runningPoints1[targetPointID].position.x) < 0.5f &&
            Mathf.Abs(transform.position.z - runningPoints1[targetPointID].position.z) < 0.5f)
            {
                if (targetPointID < runningPoints1.Count - 1)
                {
                    targetPointID++;
                    agent.SetDestination(runningPoints1[targetPointID].position);
                }
                else
                {
                    ShuffleLane();
                }

            }
        }

        if (laneID == 2)
        {
            if (Mathf.Abs(transform.position.x - runningPoints2[targetPointID].position.x) < 0.5f &&
           Mathf.Abs(transform.position.z - runningPoints2[targetPointID].position.z) < 0.5f)
            {
                if (targetPointID < runningPoints2.Count - 1)
                {
                    targetPointID++;
                    agent.SetDestination(runningPoints2[targetPointID].position);
                }
                else
                {
                    ShuffleLane();
                }

            }
        }

        if (laneID == 3)
        {
            if (Mathf.Abs(transform.position.x - runningPoints3[targetPointID].position.x) < 0.5f &&
           Mathf.Abs(transform.position.z - runningPoints3[targetPointID].position.z) < 0.5f)
            {
                if (targetPointID < runningPoints3.Count - 1)
                {
                    targetPointID++;
                    agent.SetDestination(runningPoints3[targetPointID].position);
                }
                else
                {
                    ShuffleLane();
                }

            }
        }


    }
    void InitRunningPoints()
    {
        Transform laneSet1 = GameObject.Find("FishPath_Middle").transform;
        Transform laneSet2 = GameObject.Find("FishPath_IN").transform;
        Transform laneSet3 = GameObject.Find("FishPath_OUT").transform;

        float childPoints1 = laneSet1.childCount;
        float childPoints2 = laneSet2.childCount;
        float childPoints3 = laneSet3.childCount;

        for (int i = 0; i < childPoints1; i++)
        {
            runningPoints1.Add(laneSet1.GetChild(i).transform);
        }
        for (int j = 0; j < childPoints2; j++)
        {
            runningPoints2.Add(laneSet2.GetChild(j).transform);
        }
        for (int k = 0; k < childPoints3; k++)
        {
            runningPoints3.Add(laneSet3.GetChild(k).transform);
        }
    }
    void ShuffleLane()
    {
        switching = true;
        laneID = Random.Range(1, 4);
        if (laneID == 1)
        {
            targetPointID = 0;
            agent.SetDestination(runningPoints1[0].position);
            switching = false;
        }
        if (laneID == 2)
        {
            targetPointID = 0;
            agent.SetDestination(runningPoints2[0].position);
            switching = false;
        }
        if (laneID == 3)
        {
            targetPointID = 0;
            agent.SetDestination(runningPoints3[0].position);
            switching = false;
        }

        SetLaneMaterial();
    }

    void BindLanePoints()
    {
        attackLine1.positionCount = runningPoints1.Count;
        for (int i = 0; i < runningPoints1.Count; i++)
        {
            attackLine1.SetPosition(i, runningPoints1[i].position);
        }

        attackLine2.positionCount = runningPoints2.Count;
        for (int j = 0; j < runningPoints2.Count; j++)
        {
            attackLine2.SetPosition(j, runningPoints2[j].position);
        }

        attackLine3.positionCount = runningPoints3.Count;
        for (int k = 0; k < runningPoints3.Count; k++)
        {
            attackLine3.SetPosition(k, runningPoints3[k].position);
        }
    }

    void SetLaneMaterial()
    {
        if (laneID == 1)
        {
            attackLine1.material = lineMatActive;
            attackLine2.material = lineMatBlank;
            attackLine3.material = lineMatBlank;
        }
        if (laneID == 2)
        {
            attackLine2.material = lineMatActive;
            attackLine1.material = lineMatBlank;
            attackLine3.material = lineMatBlank;
        }
        if (laneID == 3)
        {
            attackLine3.material = lineMatActive;
            attackLine2.material = lineMatBlank;
            attackLine1.material = lineMatBlank;
        }
    }



    public static void ShowAttackLane()
        {
            attackLine1.enabled = true;
            attackLine2.enabled = true;
            attackLine3.enabled = true;

        }

        public static void HideAttackLane()
        {
            attackLine1.enabled = false;
            attackLine2.enabled = false;
            attackLine3.enabled = false;
        }

    IEnumerator ReleaseWarningSonar()
    {
        while (sonar.transform.localScale.x < 10*huntRadius&&warn)
        {
            sonar.transform.localScale += new Vector3(2, 2, 2);
            yield return null;

        }
        sonarSkin.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, huntRadius);
    }
}
