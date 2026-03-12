using UnityEngine;

public class Human_WarhammerHit : MonoBehaviour
{
    [Header("Attack_test")]
    public bool isAttacking = false;

    [Header("AttackPower")]
    public float hitForce = 15f;

    [Header("PowerTransform")]
    public Transform attackerRoot;  // 캐릭터 루트(앞 방향을 쓰고 싶을 때)

    // 추가
    [Header("무기 데미지 설정")]
    public float weaponDamage = 50f;
    private void OnTriggerEnter(Collider other)
    {
        // 공격 중이 아니면 무시
        if (!isAttacking) return;

        // 이름으로 Cube_test만 맞게 하기
        if (!other.CompareTag("Cube_test")) return;

        Rigidbody rb = other.attachedRigidbody;

        // 추가
        Golem_HP golemHP = other.GetComponent<Golem_HP>();
        if (golemHP != null) golemHP.TakeDamage(weaponDamage);

        if (rb == null) return;

        // 밀어낼 방향: 캐릭터가 보는 방향 + 살짝 위로
        Vector3 dir;
        if (attackerRoot != null)
            dir = attackerRoot.forward + Vector3.up * 0.5f;
        else
            dir = (other.transform.position - transform.position).normalized + Vector3.up * 0.5f;

        rb.AddForce(dir.normalized * hitForce, ForceMode.Impulse);
    }
}
