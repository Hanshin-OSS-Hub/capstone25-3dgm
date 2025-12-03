using UnityEngine;

public class AttackHitByState : MonoBehaviour
{
    public Animator animator;           // 캐릭터 Animator
    public Human_WarhammerHit weaponHitbox;   // Human_Warhammer에 붙어있는 Hitbox

    // 공격 스테이트 이름들 (Animator 상태 이름 그대로)
    readonly int hashAttack1 = Animator.StringToHash("OneHand_Up_Attack_A_1");
    readonly int hashAttack2 = Animator.StringToHash("OneHand_Up_Attack_A_2");
    readonly int hashAttack3 = Animator.StringToHash("OneHand_Up_Attack_A_3");

    // Base Layer 번호 (기본은 0)
    public int layerIndex = 0;

    void Reset()
    {
        // 같은 오브젝트에 Animator가 있으면 자동으로 찾기
        if (animator == null)
            TryGetComponent(out animator);
    }

    void Update()
    {
        if (animator == null || weaponHitbox == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        // 현재 스테이트가 3개 공격 중 하나인지 체크
        bool inAttack =
            stateInfo.shortNameHash == hashAttack1 ||
            stateInfo.shortNameHash == hashAttack2 ||
            stateInfo.shortNameHash == hashAttack3;

        weaponHitbox.isAttacking = inAttack;
    }
}
