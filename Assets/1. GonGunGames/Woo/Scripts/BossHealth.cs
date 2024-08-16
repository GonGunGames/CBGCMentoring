using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using AllUnits;
using UnityEngine.UI;
using Unity.VisualScripting;

public class BossHealth : MonoBehaviour
{
    public int currentId; // 인스펙터에서 설정할 수 있도록 public으로 설정
    public float currentDamage; // 현재 데미지
    public float currentHealth; // 현재 체력
    public float currentDefense;
    [SerializeField] private Slider B_hpBar; // 체력바
    public bool isDead { get; private set; } = false; // 적이 사망했는지 여부
    public bool isHit = false; // 적이 피격되었는지 여부
    private CommonMob commonMob; // CommonMob 컴포넌트
    private CommonMobN commonMobN; // CommonMobN 컴포넌트
    private CommonMobB commonMobB; // CommonMobB 컴포넌트
    private Weapon weapon; // 무기 정보
    private Shotgun shotgun; // 샷건 정보
    private Sniper sniper; // 샷건 정보
    public GameObject damageTextPrefab; // 데미지 텍스트 프리팹
    public GameObject criticalDamageTextPrefab;  // 두 배 데미지 텍스트 프리팹
    public Transform damageTextSpawnPoint; // 데미지 텍스트가 생성될 위치
    public CharacterController characterController; // 캐릭터 컨트롤러
    public AudioSource hitSound;
    public AudioSource hitSound2;
    public GameObject hitEffect;
    public GameObject hitEffect2;
    public GameObject hitEffect3;
    public Boss boss;

    private ParticleSystem hitRifle;  // ParticleSystem 컴포넌트
    private ParticleSystem hitShotgun;
    private ParticleSystem hitSniper;

    private PlayerGold playerGold;
    private void Awake()
    {
        commonMob = GetComponent<CommonMob>();
        commonMobN = GetComponent<CommonMobN>();
        commonMobB = GetComponent<CommonMobB>();
        boss = GetComponent<Boss>(); // Ellite 컴포넌트 초기화
        hitRifle = hitEffect.GetComponent<ParticleSystem>(); // ParticleSystem 컴포넌트 가져오기
        hitShotgun = hitEffect2.GetComponent<ParticleSystem>(); // ParticleSystem 컴포넌트 가져오기
        hitSniper = hitEffect3.GetComponent<ParticleSystem>();
        playerGold = FindObjectOfType<PlayerGold>();
    }

    private void OnEnable()
    {
        Initialize();
        if (hitRifle != null)
        {
            hitRifle.Stop(); // ParticleSystem 중지
        }
        if (hitShotgun != null)
        {
            hitShotgun.Stop(); // ParticleSystem 중지
        }
        hitEffect.SetActive(false);
        hitEffect2.SetActive(false);
        hitEffect3.SetActive(false);
        // 무기 정보 초기화
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            weapon = player.GetComponentInChildren<Weapon>();
            shotgun = player.GetComponentInChildren<Shotgun>();
            sniper = player.GetComponentInChildren<Sniper>();
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
        }
        else if (commonMobB != null)
        {
            commonMobB.Initialize();
        };
        // 인스펙터에서 설정된 currentId를 사용하여 적 정보를 가져옵니다.
        EnemyInfo enemyInfo = DataBase.Instance.GetEnemyInfoById(currentId);

        if (enemyInfo != null)
        {
            currentId = enemyInfo.id;
            currentHealth = enemyInfo.maxHealth;
            currentDamage = enemyInfo.damage;
            currentDefense = enemyInfo.defense;
        }
        else
        {
            Debug.LogError("ID " + currentId + "의 적을 찾을 수 없습니다.");
        }
        SetMaxHealth();
        commonMobB = GetComponent<CommonMobB>();  // CommonMob 컴포넌트를 가져옵니다.
    }
    public void SetMaxHealth()
    {
        if (B_hpBar != null)
        {
            B_hpBar.maxValue = currentHealth; // 체력바의 최대값을 currentHealth로 설정
            B_hpBar.value = currentHealth; // 체력바의 현재값을 currentHealth로 설정
        }
        else
        {
            Debug.LogError("Slider B_hpBar가 할당되지 않았습니다.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 적이 총알과 충돌했을 때 처리
        if (collision.collider.CompareTag("Bullet"))
        {
            Weaponbullet bullet = collision.collider.GetComponent<Weaponbullet>();
            Weaponbullet2 bullet2 = collision.collider.GetComponent<Weaponbullet2>();

            float finalDamage = 0f;
            bool isDoubleDamage = false;

            if (bullet2 != null)
            {
                // Weaponbullet2의 폭발 범위 내의 적에게 데미지를 입히는 메서드를 호출합니다.
                hitSound2.Play();
                bullet2.NotifyExplosion();
                float bulletDamage = weapon != null ? weapon.attackDamage : 0f; // 최신 데미지를 가져옴
                finalDamage = ApplyDoubleDamage(bulletDamage, out isDoubleDamage); // 두 배의 데미지 적용
            }
            else if (bullet != null)
            {
                // Weaponbullet의 데미지를 처리합니다.
                hitSound.Play();
                hitEffect.SetActive(true);
                if (hitRifle != null)
                {
                    hitRifle.Play(); // ParticleSystem 시작
                }
                float bulletDamage = weapon != null ? weapon.attackDamage : 0f; // 최신 데미지를 가져옴
                finalDamage = ApplyDoubleDamage(bulletDamage, out isDoubleDamage); // 두 배의 데미지 적용
            }

            // 방어력 적용 후 최종 데미지로 체력 차감
            float damageAfterDefense = ApplyDamage(finalDamage);
            ShowDamageText(damageAfterDefense, isDoubleDamage); // 방어력 적용 후 데미지를 텍스트로 표시
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 적이 샷건 총알과 충돌했을 때 처리
        ShotgunBullet shotgunBullet = other.GetComponent<ShotgunBullet>();
        if (shotgunBullet != null)
        {
            hitSound.Play();
            if (hitShotgun != null)
            {
                hitShotgun.Play(); // ParticleSystem 시작
            }
            hitEffect2.SetActive(true);
            float shotgunDamage = shotgun != null ? shotgun.attackDamage : 0f; // 최신 데미지를 가져옴
            bool isDoubleDamage = false;
            float finalDamage = ApplyDoubleDamage(shotgunDamage, out isDoubleDamage); // 두 배의 데미지 적용

            // 방어력 적용 후 최종 데미지로 체력 차감
            float damageAfterDefense = ApplyDamage(finalDamage);
            ShowDamageText(damageAfterDefense, isDoubleDamage); // 방어력 적용 후 데미지를 텍스트로 표시
        }
        SniperBullet sniperBullet = other.GetComponent<SniperBullet>();
        if (sniperBullet != null)
        {
            hitSound.Play();
            if (hitShotgun != null)
            {
                hitShotgun.Play();
            }
            hitEffect3.SetActive(true);
            float sniperDamage = sniper != null ? sniper.attackDamage : 0f;
            bool isDoubleDamage = false;
            float finalDamage = ApplyDoubleDamage(sniperDamage, out isDoubleDamage);

            float damageAfterDefense = ApplyDamage(finalDamage);

            ShowDamageText(damageAfterDefense, isDoubleDamage);
        }
    }
    public float ApplyDamage(float damage)
    {
        // 방어력 적용 후 최종 데미지를 계산
        float damageAfterDefense = damage * (1 - (currentDefense / (100 + currentDefense)));

        // 소수점 첫째 자리에서 반올림
        damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10;

        // 최종 데미지를 현재 체력에서 차감
        currentHealth -= damageAfterDefense;
        B_hpBar.value = currentHealth;
        // 디버그 로그 추가
        Debug.Log($"데미지 적용: {damage} -> 방어력 적용 후: {damageAfterDefense} -> 남은 체력: {currentHealth}");

        // 적의 상태를 Hit로 전환
        commonMob?.SetState(FSMState.Hit); // CommonMob의 Hit 상태로 전환
        commonMobN?.SetState(FSMState.Hit); // CommonMobN의 Hit 상태로 전환
        commonMobB?.SetState(FSMState.Hit); // CommonMobB의 Hit 상태로 전환

        // 체력이 0 이하이고 적이 아직 사망하지 않았다면
        if (currentHealth <= 0 && !isDead)
        {
            if (playerGold != null)
            {
                int goldAmount = Random.Range(100, 200); // 10에서 100 사이의 랜덤 골드 생성
                playerGold?.AddGold(goldAmount); // 플레이어에게 골드 추가
            }
            if (characterController != null)
            {
                characterController.enabled = false;
            }
            Debug.Log("Enemy");

            // DeathCount 인스턴스를 통해 deathCount를 증가시킴
            if (DeathCount.Instance != null)
            {
                DeathCount.Instance.IncrementDeathCount();
            }

            // 적이 사망 상태임을 표시
            isDead = true;

            // Enemy 태그를 가진 모든 오브젝트의 상태를 Dead로 전환
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemies)
            {
                // CommonMob, CommonMobN, CommonMobB 상태를 Dead로 변경
                CommonMob mob = enemy.GetComponent<CommonMob>();
                if (mob != null)
                {
                    mob.SetState(FSMState.Dead);
                }

                CommonMobN mobN = enemy.GetComponent<CommonMobN>();
                if (mobN != null)
                {
                    mobN.SetState(FSMState.Dead);
                }

                CommonMobB mobB = enemy.GetComponent<CommonMobB>();
                if (mobB != null)
                {
                    mobB.SetState(FSMState.Dead);
                }
            }

            // 1초 뒤 게임 정지 호출
            Invoke("PauseGame", 1f);
        }

        return damageAfterDefense; // 최종 데미지를 반환
    }



    private float ApplyDoubleDamage(float damage, out bool isDoubleDamage)
    {
        isDoubleDamage = false;
        if (weapon != null)
        {
            isDoubleDamage = Random.value <= weapon.doubleDamageChance; // 현재 두 배의 공격력 확률 사용
            if (isDoubleDamage)
            {
                return damage * 2; // 두 배의 데미지 적용
            }
        }
        else if (shotgun != null)
        {
            isDoubleDamage = Random.value <= shotgun.doubleDamageChance; // 현재 두 배의 공격력 확률 사용
            if (isDoubleDamage)
            {
                return damage * 2; // 두 배의 데미지 적용
            }
        }
        else if (sniper != null)
        {
            isDoubleDamage = Random.value <= sniper.doubleDamageChance;
            if (isDoubleDamage)
            {
                return damage * 2;
            }
        }
        return damage; // 기본 데미지 반환
    }
    public void ShowDamageText(float damage, bool isDoubleDamage)
    {
        GameObject textPrefab = isDoubleDamage ? criticalDamageTextPrefab : damageTextPrefab;
        if (textPrefab != null && damageTextSpawnPoint != null)
        {
            // 데미지 텍스트 프리팹을 생성합니다.
            GameObject damageTextInstance = Instantiate(textPrefab, damageTextSpawnPoint.position, Quaternion.identity);

            // TextMeshPro 컴포넌트를 가져옵니다.
            DamageText damageTextScript = damageTextInstance.GetComponent<DamageText>();
            if (damageTextScript != null)
            {
                damageTextScript.SetDamage(damage); // SetDamage 호출
            }
            else
            {
                Debug.LogError("DamageText 컴포넌트를 textPrefab에서 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("textPrefab 또는 damageTextSpawnPoint가 할당되지 않았습니다.");
        }
    }
}