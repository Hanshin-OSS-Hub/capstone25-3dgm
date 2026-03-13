using UnityEngine;

public class TestSlashOnClick : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashEffect;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("좌클릭 감지됨");

            if (slashEffect != null)
            {
                slashEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                slashEffect.Play(true);
            }
        }
    }
}