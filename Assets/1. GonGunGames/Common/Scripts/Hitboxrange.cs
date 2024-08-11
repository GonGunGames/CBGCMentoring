using System.Collections;
using UnityEngine;
using AllUnits;

public class HitBoxrange : MonoBehaviour
{
    public float attackdamage;

    private MeshRenderer meshRenderer;
    public GameObject jumpEffect;
    private ParticleSystem jumpParticleSystem;
    private BossHealth bossHealth;
    public CapsuleCollider capsuleCollider;
    private bool isParticleSystemPlaying = false;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        meshRenderer = GetComponent<MeshRenderer>();
        bossHealth = GetComponentInParent<BossHealth>();// 부모 객체에서 EnemyHealth 컴포넌트를 가져옵니다.
        if (bossHealth != null)
        {
            attackdamage = bossHealth.currentDamage;
        }

        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        if (jumpEffect != null)
        {
            jumpParticleSystem = jumpEffect.GetComponent<ParticleSystem>();
            if (jumpParticleSystem == null)
            {
                Debug.LogError("No ParticleSystem component found on jumpEffect.");
            }
        }
    }

    public void EnableHitBoxrange()
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
            StartCoroutine(DisableMeshRendererAfterDelay(3.9f));
        }

        if (jumpParticleSystem != null && !isParticleSystemPlaying)
        {
            StartCoroutine(PlayParticleSystemWithDelay(2f));
        }
    }

    private IEnumerator DisableMeshRendererAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
            capsuleCollider.enabled = false;
        }
    }

    private IEnumerator PlayParticleSystemWithDelay(float delay)
    {
        isParticleSystemPlaying = true;
        yield return new WaitForSeconds(delay);
        if (jumpParticleSystem != null)
        {
            jumpParticleSystem.Play();
            capsuleCollider.enabled = true;
            StartCoroutine(StopParticleSystemAfterDelay(jumpParticleSystem.main.duration));
        }
    }

    private IEnumerator StopParticleSystemAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (jumpParticleSystem != null)
        {
            jumpParticleSystem.Stop();
            isParticleSystemPlaying = false;
        }
    }
}