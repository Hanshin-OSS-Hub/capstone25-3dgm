using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SequentialSkillEffect : MonoBehaviour
{
    [Header("Visual Effects")]
    [Tooltip("첫 번째로 재생될 이펙트")]
    public GameObject firstEffect;
    
    [Tooltip("두 번째로 재생될 이펙트")]
    public GameObject secondEffect;
    
    [Header("Settings")]
    [Tooltip("첫 번째 이펙트 지속 시간 (초)")]
    public float firstEffectDuration = 2f;
    
    [Tooltip("이펙트가 생성될 위치")]
    public Transform spawnPosition;
    
    private bool isPlaying = false;
    
    void Update()
    {
        // 1번 키를 누르면 스킬 실행
        if (Keyboard.current != null && 
            Keyboard.current.digit1Key.wasPressedThisFrame && 
            !isPlaying)
        {
            StartCoroutine(PlaySkillSequence());
        }
    }
    
    IEnumerator PlaySkillSequence()
    {
        isPlaying = true;
        
        // 생성 위치 결정 (spawnPosition이 없으면 이 오브젝트 위치)
        Vector3 pos = spawnPosition != null ? spawnPosition.position : transform.position;
        Quaternion rot = spawnPosition != null ? spawnPosition.rotation : transform.rotation;
        
        // === 첫 번째 이펙트 실행 ===
        if (firstEffect != null)
        {
            GameObject effect1 = Instantiate(firstEffect, pos, rot);
            
            // Particle System 재생
            ParticleSystem[] ps1 = effect1.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in ps1)
            {
                ps.Play();
            }
            
            Debug.Log("첫 번째 이펙트 실행!");
            
            // 첫 번째 이펙트 재생 시간만큼 대기
            yield return new WaitForSeconds(firstEffectDuration);
            
            // 첫 번째 이펙트 삭제
            Destroy(effect1);
        }
        
        // === 두 번째 이펙트 실행 ===
        if (secondEffect != null)
        {
            GameObject effect2 = Instantiate(secondEffect, pos, rot);
            
            // Particle System 재생
            ParticleSystem[] ps2 = effect2.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in ps2)
            {
                ps.Play();
            }
            
            Debug.Log("두 번째 이펙트 실행!");
            
            // 두 번째 이펙트는 3초 후 자동 삭제
            Destroy(effect2, 3f);
        }
        
        isPlaying = false;
        Debug.Log("스킬 종료!");
    }
}