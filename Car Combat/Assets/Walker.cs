using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 startPos;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPos = this.transform.position;
        InvokeRepeating("SwitchGoal", 0, 3);
    }

    void SwitchGoal()
    {
        Vector3 goal = startPos + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
        agent.SetDestination(goal);
    }


}
