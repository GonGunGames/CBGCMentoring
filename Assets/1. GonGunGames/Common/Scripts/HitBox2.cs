using System.Collections;
using UnityEngine;
using AllUnits;

public class HitBox2 : Unit
{
    public float attackdamage;

    private CapsuleCollider capsuleCollider;

    protected override void Start()
    {
        base.Start();
        attackdamage = damage;  // 'damage'가 제대로 초기화되었는지 확인

        capsuleCollider = GetComponent<CapsuleCollider>();

        // 초기 상태 비활성화
        capsuleCollider.enabled = false;
    }

    // HitBox 활성화 메서드
    public void EnableHitBox2()
    {
        capsuleCollider.enabled = true;
        StartCoroutine(DisableHitBoxAfterDelay(0.1f)); // 0.1초 후에 콜라이더 비활성화
    }

    // HitBox 비활성화 메서드 (CapsuleCollider)
    private IEnumerator DisableHitBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        capsuleCollider.enabled = false;
    }
}
