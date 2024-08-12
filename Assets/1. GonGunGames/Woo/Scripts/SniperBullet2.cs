using System.Collections;
using UnityEngine;

public class SniperBullet2 : MonoBehaviour
{
    public float destroyDelay = 2.0f; // 기본값을 더 길게 설정
    public float damage;
    private bool isDoubleDamage; // 두 배 데미지 여부
    public GameObject bulletPrefab; // 추가 총알 프리팹
    public float bulletSpeed; // 추가 총알 발사 속도

    private void Start()
    {
        damage = 0f; // 무기 공격력 설정
        StartCoroutine(DestroyAfterDelay());
    }

    public void Initialize(float baseDamage, float doubleDamageMultiplier, bool isDoubleDamage)
    {
        // 총알의 데미지를 두 배 데미지와 기본 데미지를 기반으로 설정합니다.
        damage = baseDamage * doubleDamageMultiplier;
        this.isDoubleDamage = isDoubleDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 충돌한 위치와 방향을 기준으로 3갈래로 총알 발사
            FireSplitBullets();
            Destroy(gameObject); // 현재 총알 삭제
        }
    }

    private void FireSplitBullets()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("BulletPrefab is not assigned.");
            return;
        }

        Vector3 forward = transform.forward; // 현재 총알의 방향

        // 3갈래로 총알 발사
        for (int i = 0; i < 3; i++)
        {
            float angle = i * 10 - 10; // 3개의 각도 설정
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0)); // 회전 변환
            Vector3 direction = rotation * forward; // 회전된 방향

            // 총알 생성
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(direction));
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed; // 총알의 속도 설정
            }

            SniperBullet2 newBulletScript = newBullet.GetComponent<SniperBullet2>();
            if (newBulletScript != null)
            {
                newBulletScript.Initialize(damage, 1f, isDoubleDamage); // 추가 총알의 데미지 설정
            }
        }
    }

    public IEnumerator DestroyAfterDelay()
    {
        Debug.Log("Destroying bullet in " + destroyDelay + " seconds.");
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    public void SetDestroyDelay(float delay)
    {
        destroyDelay = delay;
        Debug.Log("Updated destroy delay to " + destroyDelay + " seconds.");
    }
}
