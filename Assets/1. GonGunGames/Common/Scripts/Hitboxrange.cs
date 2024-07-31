using System.Collections;
using UnityEngine;
using AllUnits;

public class HitBoxrange : Unit
{
    public float attackdamage;

    private MeshRenderer meshRenderer;

    protected override void Start()
    {
        base.Start();
        attackdamage = damage;  // 'damage'가 제대로 초기화되었는지 확인

        meshRenderer = GetComponent<MeshRenderer>();

        // 초기 상태 비활성화
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }

    // 메시 렌더러 활성화 메서드
    public void EnableHitBoxrange()
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
            StartCoroutine(DisableMeshRendererAfterDelay(3.9f)); // 0.1초 후에 메시 렌더러 비활성화
        }
    }

    // 메시 렌더러 비활성화 메서드
    private IEnumerator DisableMeshRendererAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // 메시 렌더러 비활성화
        }
    }
}
