using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator animator;
    int hashAttackCount = Animator.StringToHash("AttackCount");

    // 콤보 관련 변수 추가
    int maxCombo = 3;             // 최대 콤보 수
    float lastAttackTime = 0f;    // 마지막 공격 시간
    float comboResetTime = 1.5f;  // 콤보가 유지되는 시간 (이 시간이 지나면 초기화)

    void Start()
    {
        TryGetComponent(out animator);
    }

    void Update()
    {
        // 1. 일정 시간이 지나면 콤보 초기화 (아무것도 안 하고 있을 때)
        if (Time.time - lastAttackTime > comboResetTime && AttackCount > 0)
        {
            AttackCount = 0; // 대기 상태로 복귀
        }

        // 2. 마우스 왼쪽 버튼 클릭
        if (Input.GetMouseButtonDown(0))
        {
            OnClickAttack();
        }
    }

    void OnClickAttack()
    {
        // 마지막 공격 시간을 현재 시간으로 갱신
        lastAttackTime = Time.time;

        // 현재 카운트를 가져옴
        int currentCount = AttackCount;

        // 콤보 증가 로직
        // 현재 0(대기)이거나 콤보 중일 때만 증가
        if (currentCount < maxCombo)
        {
            AttackCount = currentCount + 1;
        }
        else
        {
            // 이미 3타(최대)라면 다시 1타부터 시작할지, 
            // 아니면 0으로 초기화할지는 기획에 따라 다릅니다.
            // 보통은 공격이 끝날 때까지 입력을 막거나, 다시 1부터 시작합니다.
            AttackCount = 1;
        }

        Debug.Log($"공격! 현재 콤보: {AttackCount}");
    }

    public int AttackCount
    {
        get => animator.GetInteger(hashAttackCount);
        set => animator.SetInteger(hashAttackCount, value);
    }
}