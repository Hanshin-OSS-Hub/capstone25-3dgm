using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimultaneousSkillEffect : MonoBehaviour
{
    [Header("Visual Effects - Skill 2")]
    [Tooltip("2번 스킬의 첫 번째 이펙트")]
    public GameObject skill2_FirstEffect;
    
    [Tooltip("2번 스킬의 두 번째 이펙트")]
    public GameObject skill2_SecondEffect;
    
    [Header("Settings")]
    [Tooltip("이펙트가 생성될 위치")]
    public Transform spawnPosition;
    
    [Tooltip("이펙트 자동 삭제 시간 (초)")]
    public float autoDestroyTime = 3f;
    
    private bool isPlaying = false;
    
    void Update()
    {
        // 2번 키를 누르면 스킬 실행
        if (Keyboard.current != null && 
            Keyboard.current.digit2Key.wasPressedThisFrame && 
            !isPlaying)
        {
            PlaySimultaneousSkill();
        }
    }
    
    void PlaySimultaneousSkill()
    {
        isPlaying = true;
        
        // 생성 위치 결정
        Vector3 pos = spawnPosition != null ? spawnPosition.position : transform.position;
        Quaternion rot = spawnPosition != null ? spawnPosition.rotation : transform.rotation;
        
        // === 2번 스킬의 첫 번째 이펙트 실행 ===
        if (skill2_FirstEffect != null)
        {
            GameObject effect1 = Instantiate(skill2_FirstEffect, pos, rot);
            
            // Particle System 재생
            ParticleSystem[] ps1 = effect1.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in ps1)
            {
                ps.Play();
            }
            
            Debug.Log("2번 스킬 - 첫 번째 이펙트 실행!");
            
            // 자동 삭제
            Destroy(effect1, autoDestroyTime);
        }
        
        // === 2번 스킬의 두 번째 이펙트 동시 실행 ===
        if (skill2_SecondEffect != null)
        {
            GameObject effect2 = Instantiate(skill2_SecondEffect, pos, rot);
            
            // Particle System 재생
            ParticleSystem[] ps2 = effect2.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in ps2)
            {
                ps.Play();
            }
            
            Debug.Log("2번 스킬 - 두 번째 이펙트 실행!");
            
            // 자동 삭제
            Destroy(effect2, autoDestroyTime);
        }
        
        Debug.Log("2번 스킬 발동! (동시 실행)");
        
        // 짧은 딜레이 후 다시 사용 가능
        StartCoroutine(ResetCooldown());
    }
    
    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isPlaying = false;
    }
}