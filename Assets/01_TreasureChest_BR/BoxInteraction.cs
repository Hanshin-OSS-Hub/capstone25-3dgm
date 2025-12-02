using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    public GameObject chestCover;  // chest cover를 참조할 변수 (Treasure의 자식 오브젝트)
    public float rotationSpeed = 1f;  // 덮개 회전 속도
    private bool isPlayerInRange = false;  // 플레이어가 범위 내에 있는지 확인하는 변수
    private bool isOpened = false;  // 덮개가 열렸는지 여부

    private GameObject player;
    private Quaternion targetRotation;  // 목표 회전값 (-90도 회전)

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        // chestCover가 Treasure 아래 자식 오브젝트라면, 이를 FindChild를 통해 찾을 수 있습니다.
        chestCover = transform.Find("ChestCover").gameObject;  // Treasure 아래의 ChestCover 오브젝트 찾기
    }

    void Update()
    {
        // 플레이어가 범위 내에 있고, E키를 눌렀을 때 덮개가 열리도록 처리
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            isOpened = true;
            targetRotation = Quaternion.Euler(-90f, 180f, 0f);  // 덮개를 -90도 회전시킬 목표 설정
        }

        // 덮개가 열린 상태에서 회전 시작
        if (isOpened && chestCover != null)
        {
            // 현재 회전값에서 목표 회전값으로 부드럽게 회전
            chestCover.transform.rotation = Quaternion.RotateTowards(
                chestCover.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    // 플레이어가 상자 범위 안에 들어왔을 때
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  // 플레이어가 범위 내에 들어왔음
            Debug.Log("Player is near the treasure!");
        }
    }

    // 플레이어가 상자 범위 밖으로 나갔을 때
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  // 플레이어가 범위 밖으로 나갔음
            Debug.Log("Player left the treasure range.");
        }
    }
}