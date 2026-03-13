using UnityEngine;

public class WeaponEffectTest : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        Debug.Log($"[TEST] {name} - effect={(ps != null ? ps.name : "NULL")}");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ps == null)
            {
                Debug.LogWarning($"[TEST] {name} - ParticleSystem null");
                return;
            }

            ps.Clear(true);
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play(true);

            Debug.Log($"[TEST] {name} - 재생 시도 / isPlaying={ps.isPlaying}");
        }
    }
}