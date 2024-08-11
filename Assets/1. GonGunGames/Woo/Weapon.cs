using System.Collections;
using UnityEngine;

public enum UpgradeOption
{
    AttackSpeed,
    AttackDamage,
    AttackChance
}

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 기본 총알 프리팹
    public GameObject bulletPrefab2; // 발사할 두 번째 총알 프리팹
    public Transform firePoint; // 총알이 발사될 위치
    public float bulletSpeed; // 총알 발사 속도
    public float attackSpeed; // 발사 간격 (초)
    public float attackDamage; // 무기 데미지 
    public float doubleDamage; // 두 배의 공격력
    public float attackChance; // 공격 성공 확률 (0.0 ~ 1.0)
    public float bulletsPerShot; // 한 번에 발사할 총알의 개수
    public float burstInterval; // 연속 발사 간격 (초)
    public PlayerHealth health; // 플레이어 건강 상태
    public float nextFireTime; // 다음 발사 가능 시간
    private int AttackDamageCount;
    private int AttackSpeedCount;
    private int AttackChanceCount;
    public int gunId;
    public float doubleDamageChance = 0.1f; // 두 배 공격력의 초기 확률
    public GameObject particlePrefab; // 파티클 프리펩
    public float particleDuration = 1.5f; // 파티클 지속 시간
    public AudioClip fireSound; // 기본 총알 발사 소리 클립
    public AudioClip fireSound2; // 두 번째 총알 발사 소리 클립
    private AudioSource audioSource; // AudioSource 컴포넌트
    public float fireSoundVolume = 0.5f; // 기본 발사 소리 볼륨 (0.0 ~ 1.0)
    public float fireSound2Volume = 0.5f; // 두 번째 발사 소리 볼륨 (0.0 ~ 1.0)

    private Coroutine fireBurstCoroutine; // FireBurst 코루틴을 저장하기 위한 변수
    private Coroutine particleCoroutine;
    private WeaponInfo currentWeapon;

    private void Awake()
    {
        particlePrefab.SetActive(false);
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기

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
                particlePrefab.SetActive(false); // 파티클 비활성화
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
                AttackSpeedCount++;
                burstInterval -= 0.01f; // 공격 속도 증가 
                Debug.Log("공격속도가 증가했습니다. 현재 공격속도: " + attackSpeed);
                if (AttackSpeedCount % 3 == 0)
                {
                    attackSpeed -= 0.4f;
                }
                break;

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

            case UpgradeOption.AttackChance:
                AttackChanceCount++;
                attackChance += 0.1f; // 공격 성공 확률 증가
                Debug.Log("공격 성공 확률이 증가했습니다. 현재 발사 확률: " + attackChance);
                if (AttackChanceCount == 5)
                {
                    attackChance += 0.5f;
                }
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

        // 파티클 효과 활성화
        particlePrefab.SetActive(true);
        StartCoroutine(ActivateParticleEffect());

        for (int i = 0; i < bulletsPerShot; i++)
        {
            if (health != null && health.isDead)
            {
                // 플레이어가 죽으면 발사 중지
                Debug.Log("Player is dead, stopping fire burst.");
                particlePrefab.SetActive(false);
                yield break;
            }

            Fire();
            yield return new WaitForSeconds(burstInterval); // 연속 발사 간격만큼 대기
        }

        // 파티클 효과 비활성화
        particlePrefab.SetActive(false);

        fireBurstCoroutine = null; // 코루틴이 완료되면 fireBurstCoroutine을 null로 설정
    }

    private void Fire()
    {
        // 공격 성공 여부 결정
        GameObject selectedBulletPrefab = Random.value <= attackChance ? bulletPrefab2 : bulletPrefab;
        bool isDoubleDamage = Random.value <= doubleDamageChance; // 현재 두 배의 공격력 확률 사용

        // 총알 생성
        GameObject bullet = Instantiate(selectedBulletPrefab, firePoint.position, firePoint.rotation);

        // 총알에 물리적 힘 적용
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed; // 총알의 속도 설정 (firePoint의 forward 방향으로 발사)
        }

        // Weaponbullet 또는 Weaponbullet2의 컴포넌트를 가져옵니다.
        Weaponbullet weaponBullet = bullet.GetComponent<Weaponbullet>();
        Weaponbullet2 weaponBullet2 = bullet.GetComponent<Weaponbullet2>();

        if (weaponBullet != null)
        {
            weaponBullet.damage = isDoubleDamage ? attackDamage * 2 : attackDamage;
        }

        if (weaponBullet2 != null)
        {
            // 두 배 데미지를 적용할지 여부를 판단하고 데미지를 설정합니다.
            weaponBullet2.Initialize(attackDamage, isDoubleDamage ? 2f : 1f, isDoubleDamage);
        }

        // 총알 발사 소리 재생
        if (audioSource != null)
        {
            if (selectedBulletPrefab == bulletPrefab2 && fireSound2 != null)
            {
                audioSource.PlayOneShot(fireSound2, fireSound2Volume); // bulletPrefab2 사운드 재생
            }
            else if (fireSound != null)
            {
                audioSource.PlayOneShot(fireSound, fireSoundVolume); // 기본 총알 사운드 재생
            }
        }

        Debug.Log(selectedBulletPrefab == bulletPrefab2 ? "공격 성공: bulletPrefab2 발사" : "공격 실패: bulletPrefab 발사");
    }

    private IEnumerator ActivateParticleEffect()
    {
        ParticleSystem particleSystem = particlePrefab.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Play();
            Debug.Log("Particle started");
            yield return new WaitForSeconds(bulletsPerShot * burstInterval); // 총알 발사 시간 동안 대기
            particleSystem.Stop();
            Debug.Log("Particle stopped");
        }
        else
        {
            Debug.LogError("No ParticleSystem component found on the particlePrefab.");
        }
    }
}
