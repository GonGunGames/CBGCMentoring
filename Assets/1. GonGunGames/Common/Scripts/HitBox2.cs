using System.Collections;
using UnityEngine;
using AllUnits;

public class HitBox2 : MonoBehaviour
{
    public float attackdamage;  // 공격력 변수
    public CapsuleCollider capsuleCollider;
    private EnemyHealth enemyHealth;  // EnemyHealth 컴포넌트 참조 변수
    private ElliteHealth elliteHealth;
    private BossHealth bossHealth;

    // 초기화 메서드
    void Start()
    {
        capsuleCollider.enabled = false;
        enemyHealth = GetComponentInParent<EnemyHealth>();
        elliteHealth = GetComponentInParent<ElliteHealth>();
        bossHealth = GetComponentInParent<BossHealth>();// 부모 객체에서 EnemyHealth 컴포넌트를 가져옵니다.

        if (enemyHealth != null)
        {
            attackdamage = enemyHealth.currentDamage;  // EnemyHealth의 현재 데미지를 가져옵니다.
        }
        else if (elliteHealth != null)
        {
            attackdamage = elliteHealth.currentDamage;
        }
        else if (bossHealth != null)
        {
            attackdamage = bossHealth.currentDamage;
        }
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
