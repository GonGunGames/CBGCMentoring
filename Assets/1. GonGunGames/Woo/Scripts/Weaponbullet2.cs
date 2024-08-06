using System.Collections;
using UnityEngine;

public class Weaponbullet2 : MonoBehaviour
{
    private float destroyDelay = 2f; // 자동 파괴 지연 시간
    public GameObject explosionPrefab; // 폭발 효과 프리팹
    private GameObject explosionInstance; // 생성된 폭발 효과 인스턴스
    public float explosionRadius = 5f; // 폭발 범위 반경
    public Weapon weapon; // Weapon 컴포넌트
    public float damage; // 기본 데미지

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Explode();
            Destroy(gameObject); // 총알 파괴
        }
    }

    private void Explode()
    {
        // 폭발 효과 생성
        if (explosionPrefab != null)
        {
            explosionInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosionInstance, destroyDelay); // 폭발 효과 파괴
        }
        else
        {
            Debug.LogWarning("Explosion prefab is not assigned.");
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    public void Initialize(float baseDamage, float doubleDamageMultiplier)
    {
        // 총알의 데미지를 두 배 데미지와 기본 데미지를 기반으로 설정합니다.
        damage = baseDamage * doubleDamageMultiplier;
    }

    public void NotifyExplosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // 다양한 적 클래스 타입 체크
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    // 폭발 데미지 적용
                    enemyHealth.ApplyDamage(damage);
                    enemyHealth.ShowDamageText(damage); // 데미지 텍스트 표시
                }

                BossHealth bossHealth = hitCollider.GetComponent<BossHealth>();
                if (bossHealth != null)
                {
                    // 폭발 데미지 적용
                    bossHealth.ApplyDamage(damage);
                    bossHealth.ShowDamageText(damage); // 데미지 텍스트 표시
                }

                ElliteHealth eliteHealth = hitCollider.GetComponent<ElliteHealth>();
                if (eliteHealth != null)
                {
                    // 폭발 데미지 적용
                    eliteHealth.ApplyDamage(damage);
                    eliteHealth.ShowDamageText(damage); // 데미지 텍스트 표시
                }
            }
        }
    }
}