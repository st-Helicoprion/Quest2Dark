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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
