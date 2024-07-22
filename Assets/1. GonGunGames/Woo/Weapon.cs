using UnityEngine;
using System.Collections;

public enum UpgradeOption
{
    AttackSpeed,
    AttackDamage,
    AttackChance
}

public class Weapon : PlayerStats
{
    public GameObject bulletPrefab; // 발사할 기본 총알 프리팹
    public GameObject bulletPrefab2; // 발사할 두 번째 총알 프리팹
    public Transform firePoint; // 총알이 발사될 위치
    public float bulletSpeed = 10f; // 총알 발사 속도
    public float attackSpeed = 1f; // 발사 간격 (초)
    public float attackDamage = 1f; // 무기 데미지 
    public float attackChance = 0.5f; // 공격 성공 확률 (0.0 ~ 1.0)
    public int bulletsPerShot = 5; // 한 번에 발사할 총알의 개수
    public float burstInterval = 0.1f; // 연속 발사 간격 (초)
    public PlayerHealth health;
    public PlayerStats stats;
    private float nextFireTime = 0f; // 다음 발사 가능 시간
    private Coroutine fireBurstCoroutine; // FireBurst 코루틴을 저장하기 위한 변수

    public void Start()
    {
        attackChance = 0f;
        attackSpeed = 1.0f;
        attackDamage = 10; // 무기 공격력 설정
    }

    private void Update()
    {
        if (health != null && health.isDead)
        {
            Debug.Log("Fireburst Stop");
            if (fireBurstCoroutine != null)
            {
                StopCoroutine(fireBurstCoroutine); // FireBurst 코루틴 중지
                fireBurstCoroutine = null;
            }
            return; // health.isDead가 true이면 이후 코드는 실행되지 않음
        }

        // 현재 시간이 nextFireTime을 초과할 때만 발사
        if (Time.time >= nextFireTime && fireBurstCoroutine == null)
        {
            fireBurstCoroutine = StartCoroutine(FireBurst()); // 연속 발사 코루틴 시작
            nextFireTime = Time.time + attackSpeed; // 다음 발사 시간 설정
        }
    }

    public void UpgradeStat(UpgradeOption option)
    {
        switch (option)
        {
            case UpgradeOption.AttackSpeed:
                attackSpeed -= 0.1f; // 공격 속도 증가 
                Debug.Log("공격속도가 증가했습니다. 현재 공격속도: " + attackSpeed);
                break;
            case UpgradeOption.AttackDamage:
                attackDamage += 5; // 공격력 증가
                Debug.Log("공격력이 증가했습니다. 현재 공격력: " + attackDamage);
                break;
            case UpgradeOption.AttackChance:
                attackChance += 0.1f; // 고폭유탄 발사확률 증가
                Debug.Log("고폭유탄 발사확률이 증가했습니다. 현재 발사 확률: " + attackChance);
                break;
            default:
                Debug.LogError("잘못된 선택입니다.");
                break;
        }
    }

    private IEnumerator FireBurst()
    {
        if (bulletPrefab == null || bulletPrefab2 == null)
        {
            Debug.LogWarning("BulletPrefabs are not assigned.");
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

        fireBurstCoroutine = null; // 코루틴이 완료되면 fireBurstCoroutine을 null로 설정
    }

    private void Fire()
    {
        // 공격 성공 여부 결정
        GameObject selectedBulletPrefab = Random.value <= attackChance ? bulletPrefab2 : bulletPrefab;

        // 총알 생성
        GameObject bullet = Instantiate(selectedBulletPrefab, firePoint.position, firePoint.rotation);

        // 총알에 물리적 힘 적용
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed; // 총알의 속도 설정 (firePoint의 forward 방향으로 발사)
        }

        Debug.Log(selectedBulletPrefab == bulletPrefab2 ? "공격 성공: bulletPrefab2 발사" : "공격 실패: bulletPrefab 발사");
    }
}