using System.Collections;
using UnityEngine;
using AllUnits;

public class HitBox : Unit
{
    public float attackdamage;

    private BoxCollider boxCollider;

    protected override void Start()
    {
        base.Start();
        attackdamage = damage;  // 'damage'가 제대로 초기화되었는지 확인

        boxCollider = GetComponent<BoxCollider>();

        // 초기 상태 비활성화

        boxCollider.enabled = false;
    }

    // HitBox 활성화 메서드
    public void EnableHitBox()
    {

        boxCollider.enabled = true;
        StartCoroutine(DisableHitBoxAfterDelay(0.1f)); // 0.1초 후에 비활성화
    }

    // HitBox 비활성화 메서드
    private IEnumerator DisableHitBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        boxCollider.enabled = false;
    }
}
