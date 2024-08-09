using System.Collections;
using UnityEngine;
using AllUnits;

public class HitBox : MonoBehaviour
{
    public float attackdamage;  // 공격력 변수
    private BoxCollider boxCollider;
    private EnemyHealth enemyHealth;  // EnemyHealth 컴포넌트 참조 변수
    private ElliteHealth elliteHealth;
    private BossHealth bossHealth;

    // 초기화 메서드
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

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

        // 초기 상태 비활성화
        boxCollider.enabled = false;
    }

    // HitBox 활성화 메서드
    public void EnableHitBox()
    {
        boxCollider.enabled = true;
        StartCoroutine(DisableHitBoxAfterDelay(0.1f));  // 0.1초 후에 비활성화
    }

    // HitBox 비활성화 메서드
    private IEnumerator DisableHitBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        boxCollider.enabled = false;
    }
}