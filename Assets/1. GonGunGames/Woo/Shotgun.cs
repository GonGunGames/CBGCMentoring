using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 기본 총알 프리팹
    public Transform firePoint; // 총알이 발사될 위치
    public float bulletSpeed; // 총알 발사 속도
    public float attackSpeed; // 발사 간격 (초)
    public float attackDamage; // 무기 데미지 
    public float attackChance; // 공격 성공 확률 (0.0 ~ 1.0)
    public float bulletsPerShot; // 한 번에 발사할 총알의 개수
    public float burstInterval; // 연속 발사 간격 (초)
    public float doubleDamage; // 두 배의 공격력
    public float doubleDamageChance = 0.1f; // 두 배 공격력의 초기 확률
    public float destroyDelay = 2.0f; // 기본값을 더 길게 설정
    public PlayerHealth health;
    public float nextFireTime; // 다음 발사 가능 시간
    private int AttackDamageCount;
    private int AttackSpeedCount;
    private int AttackChanceCount;
    public int gunId;
    public AudioClip fireSound; // 기본 총알 발사 소리 클립
    public float fireSoundVolume = 0.5f; // 기본 발사 소리 볼륨 (0.0 ~ 1.0)
    private AudioSource audioSource; // AudioSource 컴포넌트
    private Coroutine fireBurstCoroutine; // FireBurst 코루틴을 저장하기 위한 변수
    private WeaponInfo currentWeapon;
    private float additionalFireChance; // 추가 발사 확률

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
        destroyDelay = 0.2f;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 없으면 추가
        }

        // WeaponInfo에서 값을 가져와서 클래스 필드에 할당
        bulletSpeed = GetPlayerInfo.instance.GetStat(StatType.BulletSpeed);
        attackChance = GetPlayerInfo.instance.GetStat(StatType.GrenadeProbability);
        attackSpeed = GetPlayerInfo.instance.GetStat(StatType.AttackSpeed);
        attackDamage = GetPlayerInfo.instance.GetStat(StatType.Attack);
        doubleDamage = attackDamage * 2; // 두 배의 공격력
        bulletsPerShot = GetPlayerInfo.instance.GetStat(StatType.MagazineSize);
        burstInterval = GetPlayerInfo.instance.GetStat(StatType.ReloadTime);
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
            case UpgradeOption.AttackDamage:
                AttackDamageCount++;
                attackDamage += 20; // 공격력 증가
                doubleDamage = attackDamage * 2; // 두 배의 공격력 업데이트
                Debug.Log("공격력이 증가했습니다. 현재 공격력: " + attackDamage);

                if (AttackDamageCount % 3 == 0)
                {
                    attackDamage += 50;
                    doubleDamageChance = Mathf.Min(doubleDamageChance + 0.1f, 1.0f); // 두 배 공격력 확률 증가
                }
                break;

            case UpgradeOption.AttackSpeed:
                AttackSpeedCount++;
                additionalFireChance += 0.2f; // 추가 발사 확률 증가
                Debug.Log("추가 발사 확률이 증가했습니다. 현재 추가 발사 확률: " + additionalFireChance);

                if (AttackSpeedCount == 5)
                {
                    additionalFireChance += 0.3f;
                }
                break;

            case UpgradeOption.AttackChance:
                AttackChanceCount++;
                destroyDelay += 0.1f; // ShotgunBullet의 destroyDelay 증가
                Debug.Log("총알의 생존 시간이 증가했습니다. 현재 생존 시간: " + destroyDelay);

                if (AttackChanceCount == 5)
                {
                    destroyDelay += 0.5f;
                }
                break;

            default:
                Debug.LogError("잘못된 선택입니다.");
                break;
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

        Fire();

        // 추가 발사 확률이 있는 경우 한 번 더 발사
        if (Random.value < additionalFireChance)
        {
            yield return new WaitForSeconds(burstInterval);
            Fire();
        }

        fireBurstCoroutine = null; // 코루틴이 완료되면 fireBurstCoroutine을 null로 설정
    }

    private void Fire()
    {
        // 공격 성공 여부 결정
        bool isDoubleDamage = Random.value <= doubleDamageChance; // 두 배의 공격력 확률 계산

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 총알에 물리적 힘 적용
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed; // 총알의 속도 설정 (firePoint의 forward 방향으로 발사)
        }

        // 총알의 ShotgunBullet 컴포넌트에 데미지 및 destroyDelay 설정
        ShotgunBullet shotgunBullet = bullet.GetComponent<ShotgunBullet>();

        if (shotgunBullet != null)
        {
            shotgunBullet.SetDestroyDelay(destroyDelay);
            shotgunBullet.damage = isDoubleDamage ? attackDamage * 2 : attackDamage; // 두 배의 데미지를 적용
        }

        // 발사 소리 재생
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound, fireSoundVolume);
        }

        Debug.Log(isDoubleDamage ? "두 배 공격력 발사" : "기본 공격력 발사");
    }
}
