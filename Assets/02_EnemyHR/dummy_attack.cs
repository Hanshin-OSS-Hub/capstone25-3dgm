using UnityEngine;

public class dummy_attack : MonoBehaviour
{
    [Header("무기 공격력")]
    public int damage = 50;

    private void OnTriggerEnter(Collider other)
    {
        // 부딪힌 대상한테 합쳐진 Golem_HP 스크립트가 있는지 확인
        Golem_HP enemy = other.GetComponent<Golem_HP>();

        // 스크립트가 있다면 때린다!
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}