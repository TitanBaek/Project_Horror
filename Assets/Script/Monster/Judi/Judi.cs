using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Judi : Monster
{
    private NavMeshAgent agent;
    private Transform playerPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private void Update()
    {
        agent.destination= playerPos.position;
    }
    protected override void Attack()
    {
    }
}
