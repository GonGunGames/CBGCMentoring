using System.Collections;
using UnityEngine;

public class Weaponbullet2 : MonoBehaviour
{
    private float destroyDelay = 2f; // 자동 파괴 지연 시간
    public GameObject explosionPrefab; // 폭발 효과 프리팹
    private GameObject explosionInstance; // 생성된 폭발 효과 인스턴스
    public float explosionRadius = 5f; // 폭발 범위 반경
    public Weapon weapon;

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

    public void NotifyExplosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    float explosionDamage = weapon.attackDamage; // 폭발 데미지 설정
                    enemyHealth.ApplyDamage(explosionDamage);
                }
            }
        }
    }
}