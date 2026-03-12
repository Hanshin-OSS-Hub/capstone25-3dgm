using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Golem_HP : MonoBehaviour
{
    [Header("HP 설정")]
    public float maxHP = 300f;
    public float currentHP;

    [Header("피격 설정")]
    public float hitCooldown = 0.5f;

    private float lastHitTime = -999f;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Golem_attack golemAttack;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        golemAttack = GetComponent<Golem_attack>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        if (Time.time < lastHitTime + hitCooldown) return;

        lastHitTime = Time.time;
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log($"골렘 피격! 현재 HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit2");
            StartCoroutine(DisableAgentBriefly());
        }
    }

    IEnumerator DisableAgentBriefly()
    {
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(0.3f);
        if (!isDead) navMeshAgent.isStopped = false;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("골렘 사망!");

        if (golemAttack != null) golemAttack.enabled = false;
        if (navMeshAgent != null) navMeshAgent.enabled = false;

        animator.SetTrigger("Die");
        StartCoroutine(DisableAnimatorAfterDeath());

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 3f);
    }

    IEnumerator DisableAnimatorAfterDeath()
    {
        yield return new WaitForSeconds(2.5f); // Death 애니메이션 길이에 맞게 조정
        animator.enabled = false;
    }
}