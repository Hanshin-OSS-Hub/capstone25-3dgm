using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    Animator animator;

    // Animator 파라미터 해시
    int hashWeaponState = Animator.StringToHash("WeaponState");
    int hashChangeWeapon = Animator.StringToHash("ChangeWeapon"); // Trigger, 코드 1차 수정

    // 상태 값 정의
    const int STATE_UNARMED = 0;   // 맨손 (Idle_Weapon)
    const int STATE_WEAPON = 1;   // 무기 든 상태 (Idle_Unarmed)

    void Start()
    {
        TryGetComponent(out animator);

        // 시작 상태 (원하는 쪽으로 선택)
        // 무기 든 상태로 시작
        WeaponState = STATE_WEAPON;
    }

    void Update()
    {
        // 마우스 우클릭 (오른쪽 버튼 Down 1번)
        if (Input.GetMouseButtonDown(1))
        {
            ToggleWeapon();
        }
    }

    void ToggleWeapon()
    {
        if (WeaponState == STATE_WEAPON)
        {
            // 무기 → 맨손
            WeaponState = STATE_UNARMED;
            Debug.Log("무기 → 맨손 변환");
        }
        else
        {
            // 맨손 → 무기
            WeaponState = STATE_WEAPON;
            Debug.Log("맨손 → 무기 변환");
        }

        //"변환 시작하라" 트리거만 쏴줌
        animator.SetTrigger(hashChangeWeapon);
    }

    // Animator int 파라미터 직접 제어용 프로퍼티
    public int WeaponState
    {
        get => animator.GetInteger(hashWeaponState);
        set => animator.SetInteger(hashWeaponState, value);
    }
}
