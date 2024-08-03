using UnityEngine;
using TMPro;  // TextMeshPro 네임스페이스 추가
using AllUnits;
using System.Collections;

public class ElliteHealth : MonoBehaviour
{
    public int currentId;  // 인스펙터에서 설정할 수 있도록 public으로 설정
    public float currentDamage;  // 현재 데미지
    public float currentHealth;  // 현재 체력
    public bool isDead { get; private set; } = false;  // 적이 사망했는지 여부
    private CommonMob commonMob;  // CommonMob 컴포넌트
    private CommonMobN commonMobN;  // CommonMobN 컴포넌트
    private CommonMobB commonMobB;  // CommonMobB 컴포넌트
    private Weapon weapon;  // 무기 정보
    private Shotgun shotgun;  // 샷건 정보
    public GameObject damageTextPrefab;  // 데미지 텍스트 프리팹
    public Transform damageTextSpawnPoint;  // 데미지 텍스트가 생성될 위치
    [SerializeField] private GameObject deathPrefab; // Dead 상태에서 스폰할 프리팹
    [SerializeField] private GameObject goldPrefab; // Dead 상태에서 스폰할 프리팹
    private int deathCount;
    private Ellite ellite;
    public CharacterController characterController; // 캐릭터 컨트롤러

    private void Awake()
    {
        commonMob = GetComponent<CommonMob>();
        commonMobN = GetComponent<CommonMobN>();
        commonMobB = GetComponent<CommonMobB>();
    }

    private void Start()
    {
        Initialize();

        // 적의 상태를 Idle로 설정
        ellite = GetComponent<Ellite>();
        // 무기 정보 초기화
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            weapon = player.GetComponentInChildren<Weapon>();
            shotgun = player.GetComponentInChildren<Shotgun>();
        }
        else
        {
            Debug.LogError("Player를 찾을 수 없습니다.");
        }
    }

    public void Initialize()
    {
        isDead = false;

        if (commonMob != null)
        {
            commonMob.Initialize();
        }
        else if (commonMobN != null)
        {
            commonMobN.Initialize();
        };
        // 인스펙터에서 설정된 currentId를 사용하여 적 정보를 가져옵니다.
        EnemyInfo enemyInfo = DataBase.Instance.GetEnemyInfoById(currentId);

        if (enemyInfo != null)
        {
            currentId = enemyInfo.id;
            currentHealth = enemyInfo.maxHealth;
            currentDamage = enemyInfo.damage;
        }
        else
        {
            Debug.LogError("ID " + currentId + "의 적을 찾을 수 없습니다.");
        }

        commonMob = GetComponent<CommonMob>();  // CommonMob 컴포넌트를 가져옵니다.
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 적이 총알과 충돌했을 때 처리
        if (collision.collider.CompareTag("Bullet"))
        {
            Weaponbullet bullet = collision.collider.GetComponent<Weaponbullet>();
            Weaponbullet2 bullet2 = collision.collider.GetComponent<Weaponbullet2>();

            if (bullet2 != null)
            {
                // Weaponbullet2의 폭발 범위 내의 적에게 데미지를 입히는 메서드를 호출합니다.
                float bulletDamage = weapon != null ? weapon.attackDamage : 0f; // 최신 데미지를 가져옴
                float finalDamage = ApplyDoubleDamage(bulletDamage); // 두 배의 데미지 적용
                ShowDamageText(finalDamage); // 두 배의 데미지를 텍스트로 표시
                bullet2.NotifyExplosion();
            }
            else if (bullet != null)
            {
                // Weaponbullet의 데미지를 처리합니다.
                float bulletDamage = weapon != null ? weapon.attackDamage : 0f; // 최신 데미지를 가져옴
                float finalDamage = ApplyDoubleDamage(bulletDamage); // 두 배의 데미지 적용
                ShowDamageText(finalDamage); // 두 배의 데미지를 텍스트로 표시
                ApplyDamage(finalDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적이 샷건 총알과 충돌했을 때 처리
        ShotgunBullet shotgunBullet = other.GetComponent<ShotgunBullet>();
        if (shotgunBullet != null)
        {
            float shotgunDamage = shotgun != null ? shotgun.attackDamage : 0f; // 최신 데미지를 가져옴
            ShowDamageText(shotgunDamage);
            ApplyDamage(shotgunDamage);
        }
    }

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null && damageTextSpawnPoint != null)
        {
            // 데미지 텍스트 프리팹을 생성합니다.
            GameObject damageTextInstance = Instantiate(damageTextPrefab, damageTextSpawnPoint.position, Quaternion.identity);

            // TextMeshPro 컴포넌트를 가져옵니다.
            DamageText damageTextScript = damageTextInstance.GetComponent<DamageText>();
            if (damageTextScript != null)
            {
                damageTextScript.SetDamage(damage); // SetDamage 호출
            }
            else
            {
                Debug.LogError("DamageText 컴포넌트를 damageTextPrefab에서 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("damageTextPrefab 또는 damageTextSpawnPoint가 할당되지 않았습니다.");
        }
    }
    private float ApplyDoubleDamage(float damage)
    {
        if (weapon != null)
        {
            bool isDoubleDamage = Random.value <= weapon.doubleDamageChance; // 현재 두 배의 공격력 확률 사용
            if (isDoubleDamage)
            {
                return damage * 2; // 두 배의 데미지 적용
            }
        }
        return damage; // 기본 데미지 반환
    }
    public void ApplyDamage(float damage)
    {
        Debug.Log("데미지 적용: " + damage); // 디버그 로그 추가
        currentHealth -= damage;
        commonMob?.SetState(FSMState.Hit); // CommonMob의 Hit 상태로 전환
        commonMobN?.SetState(FSMState.Hit); // CommonMobN의 Hit 상태로 전환
        commonMobB?.SetState(FSMState.Hit); // CommonMobB의 Hit 상태로 전환
        if (currentHealth <= 0 && !isDead)
        {

            Instantiate(deathPrefab, transform.position, transform.rotation);
            Instantiate(goldPrefab, transform.position, transform.rotation);
            isDead = true;

            // 적 사망 시 추가 로직 처리 (예: 애니메이션, 아이템 드랍 등)
            StartCoroutine(HandleDeath());
        }
    }
    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1f);
        ellite.DeadEllite();
        // 프리팹 인스턴스화
        if (characterController != null)
        {
            characterController.enabled = false;
        }
        deathCount++;
        Debug.Log("Enemy");

        // DeathCount 인스턴스를 통해 deathCount를 증가시킴
        if (DeathCount.Instance != null)
        {
            DeathCount.Instance.IncrementDeathCount();
        }
    }

}