using UnityEngine;
using UnityEngine.AI;

public class Golem_attack : MonoBehaviour
{
    [Header("타겟 설정")]
    public Transform playerTarget;      // 플레이어 (드래그해서 넣기)
    public Animator animator;           // 골렘의 애니메이터 (드래그해서 넣기)

    [Header("능력치 설정")]
    public float attackRange = 2.5f;    // 공격 사거리
    public float attackCooldown = 2.0f; // 공격 속도 (초)

    private NavMeshAgent navMeshAgent;
    private float lastAttackTime = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // 만약 인스펙터에서 깜빡하고 안 넣었으면 자동으로 찾기
        if (animator == null) animator = GetComponent<Animator>();

        // 플레이어를 못 넣었으면 태그로 찾기 (플레이어 태그가 Player여야 함)
        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTarget = p.transform;
        }
    }

    void Update()
    {
        if (playerTarget == null || navMeshAgent == null) return;

        float distance = Vector3.Distance(transform.position, playerTarget.position);

        // 1. 사거리보다 멀면 -> 이동
        if (distance > attackRange)
        {
            navMeshAgent.isStopped = false;

            navMeshAgent.SetDestination(playerTarget.position);

            animator.SetBool("Walk", true);
        }
        // 2. 사거리 안이면 -> 멈춤 & 공격
        else
        {
            navMeshAgent.isStopped = true;

            // 걷기 끄기
            animator.SetBool("Walk", false);

            // 쿨타임 체크 후 공격
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
            }

            // 플레이어 바라보기
            LookAtPlayer();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        animator.SetTrigger("Rage");

        Debug.Log("골렘 공격!");
    }

    void LookAtPlayer()
    {
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}