using UnityEngine;
using UnityEngine.AI;

public class Enemy_move : MonoBehaviour
{
    public Transform playerTarget;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (playerTarget != null && navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(playerTarget.position);
        }
    }
}