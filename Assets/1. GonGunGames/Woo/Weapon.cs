using UnityEngine;
using System.Collections;
public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint; // 총알이 발사될 위치
    public float bulletSpeed = 10f; // 총알 발사 속도
    public float fireRate = 1f; // 발사 간격 (초)
    public int bulletsPerShot = 5; // 한 번에 발사할 총알의 개수
    public float burstInterval = 0.1f; // 연속 발사 간격 (초)

    private float nextFireTime = 0f; // 다음 발사 가능 시간

    private void Update()
    {
        // 현재 시간이 nextFireTime을 초과할 때만 발사
        if (Time.time >= nextFireTime)
        {
            StartCoroutine(FireBurst()); // 연속 발사 코루틴 시작
            nextFireTime = Time.time + fireRate; // 다음 발사 시간 설정
        }
    }

    private IEnumerator FireBurst()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("BulletPrefab is not assigned.");
            yield break;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint is not assigned.");
            yield break;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Fire();
            yield return new WaitForSeconds(burstInterval); // 연속 발사 간격만큼 대기
        }
    }

    private void Fire()
    {
        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 총알에 물리적 힘 적용
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed; // 총알의 속도 설정 (firePoint의 forward 방향으로 발사)
        }
    }
}