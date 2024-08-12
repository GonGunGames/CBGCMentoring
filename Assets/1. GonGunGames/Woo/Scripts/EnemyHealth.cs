using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int currentId;
    public float currentDamage;
    public float currentHealth;
    public float currentDefense;
    public bool isDead { get; private set; } = false;
    private CommonMob commonMob;
    private CommonMobN commonMobN;
    private CommonMobB commonMobB;
    private Weapon weapon;
    private Shotgun shotgun;
    private Sniper sniper;
    [SerializeField] private GameObject deathPrefab;
    public GameObject damageTextPrefab;
    public GameObject criticalDamageTextPrefab;
    public Transform damageTextSpawnPoint;
    public int deathCount;
    public CharacterController characterController;
    public AudioSource hitSound;
    public AudioSource hitSound2;
    public GameObject hitEffect;
    public GameObject hitEffect2;
    public GameObject hitEffect3;

    private ParticleSystem hitRifle;
    private ParticleSystem hitShotgun;
    private ParticleSystem hitSniper;

    // PlayerGold 참조0
    private PlayerGold playerGold;

    void Awake()
    {
        commonMob = GetComponent<CommonMob>();
        commonMobN = GetComponent<CommonMobN>();
        commonMobB = GetComponent<CommonMobB>();
        hitRifle = hitEffect.GetComponent<ParticleSystem>();
        hitShotgun = hitEffect2.GetComponent<ParticleSystem>();
        hitSniper = hitEffect3.GetComponent<ParticleSystem>();
        playerGold = FindObjectOfType<PlayerGold>();
    }

    private void OnEnable()
    {
        if (hitRifle != null)
        {
            hitRifle.Stop();
        }
        if (hitShotgun != null)
        {
            hitShotgun.Stop();
        }
        hitEffect.SetActive(false);
        hitEffect2.SetActive(false);
        hitEffect3.SetActive(false);
        Initialize();

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


    public void Update()
    {
        if (enabled != true) {
            currentHealth *= 1.5f;
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
        commonMob = GetComponent<CommonMob>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Weaponbullet bullet = collision.collider.GetComponent<Weaponbullet>();
            Weaponbullet2 bullet2 = collision.collider.GetComponent<Weaponbullet2>();

            float finalDamage = 0f;
            bool isDoubleDamage = false;

            if (bullet2 != null)
            {
                hitSound2.Play();
            }
            else if (bullet != null)
            {
                hitSound.Play();
                hitEffect.SetActive(true);
                if (hitRifle != null)
                {
                    hitRifle.Play();
                }
                float bulletDamage = weapon != null ? weapon.attackDamage : 0f;
                finalDamage = ApplyDoubleDamage(bulletDamage, out isDoubleDamage);

                float damageAfterDefense = ApplyDamage(finalDamage);
                ShowDamageText(damageAfterDefense, isDoubleDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ShotgunBullet shotgunBullet = other.GetComponent<ShotgunBullet>();
        if (shotgunBullet != null)
        {
            hitSound.Play();
            if (hitShotgun != null)
            {
                hitShotgun.Play();
            }
            hitEffect2.SetActive(true);
            float shotgunDamage = shotgun != null ? shotgun.attackDamage : 0f;
            bool isDoubleDamage = false;
            float finalDamage = ApplyDoubleDamage(shotgunDamage, out isDoubleDamage);

            float damageAfterDefense = ApplyDamage(finalDamage);

            ShowDamageText(damageAfterDefense, isDoubleDamage);
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
        float damageAfterDefense = damage * (1 - (currentDefense / (100 + currentDefense)));
        damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10;
        currentHealth -= damageAfterDefense;

        Debug.Log($"데미지 적용: {damage} -> 방어력 적용 후: {damageAfterDefense} -> 남은 체력: {currentHealth}");

        commonMob?.SetState(FSMState.Hit);
        commonMobN?.SetState(FSMState.Hit);
        commonMobB?.SetState(FSMState.Hit);

        if (currentHealth <= 0 && !isDead)
        {

            if (playerGold != null)
            {
                int goldAmount = Random.Range(11, 20); // 10에서 100 사이의 랜덤 골드 생성
                playerGold?.AddGold(goldAmount); // 플레이어에게 골드 추가
            }
            Instantiate(deathPrefab, transform.position, transform.rotation);
            deathCount++;
            if (characterController != null)
            {
                characterController.enabled = false;
            }
            Debug.Log("Enemy");

            if (DeathCount.Instance != null)
            {
                DeathCount.Instance.IncrementDeathCount();
            }

            isDead = true;
        }

        return damageAfterDefense;
    }

    private float ApplyDoubleDamage(float damage, out bool isDoubleDamage)
    {
        isDoubleDamage = false;
        if (weapon != null)
        {
            isDoubleDamage = Random.value <= weapon.doubleDamageChance;
            if (isDoubleDamage)
            {
                return damage * 2;
            }
        }
        else if (shotgun != null)
        {
            isDoubleDamage = Random.value <= shotgun.doubleDamageChance;
            if (isDoubleDamage)
            {
                return damage * 2;
            }
        }
        return damage;
    }

    public void ShowDamageText(float damage, bool isDoubleDamage)
    {
        GameObject textPrefab = isDoubleDamage ? criticalDamageTextPrefab : damageTextPrefab;
        if (textPrefab != null && damageTextSpawnPoint != null)
        {
            GameObject damageTextInstance = Instantiate(textPrefab, damageTextSpawnPoint.position, Quaternion.identity);
            DamageText damageTextScript = damageTextInstance.GetComponent<DamageText>();
            if (damageTextScript != null)
            {
                damageTextScript.SetDamage(damage);
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
