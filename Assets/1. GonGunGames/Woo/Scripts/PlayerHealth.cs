using System.Collections;
using UnityEngine;
using AllUnits;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float currentDefense;
    bool isDamage;
    private CameraFollow cameraFollow;  // CameraFollow 스크립트 참조

    private Rigidbody rb;
    private Animator myAnim;
    private Renderer playerRenderer;  // 플레이어의 Renderer 컴포넌트 참조
    [SerializeField] private Slider _hpBar;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Slider exp;
    public bool isDead { get; private set; } = false;
    public bool isHit = false;
    public float deathTime;
    private float lastDamageTime = 0f; // 마지막 데미지 시간
    private float damageInterval = 1f; // 데미지 간격 (초)
    public GameObject damageTextPrefab;  // 데미지 텍스트 프리팹
    public Transform damageTextSpawnPoint;  // 데미지 텍스트가 생성될 위치
    public AudioSource audioSource;
    public Collider collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        playerRenderer = GetComponent<Renderer>(); // Renderer 컴포넌트 가져오기
    }

    private void Start()
    {
        //currentHealth = DataBase.Instance.playerData.maxHealth;
        currentHealth = GetPlayerInfo.instance.GetStat(StatType.Health);
        currentDefense = GetPlayerInfo.instance.GetStat(StatType.Defense);
        SetMaxHealth(currentHealth);

    }

    private void Update()
    {
        _hpBar.value = currentHealth;
    }

    public void SetMaxHealth(float hp)
    {
        _hpBar.maxValue = hp;
        _hpBar.value = hp;
    }

    public void HealToMax()
    {
        currentHealth = GetPlayerInfo.instance.GetStat(StatType.Health); // 체력을 최대 값으로 설정
        Debug.Log("Player healed to max health.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && !isDamage)
        {
            isDamage = true;

            // 적의 damage 값을 가져오기 위해 EnemyHealth, ElliteHealth, BossHealth 스크립트를 참조합니다.
            EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
            ElliteHealth ellite = collision.collider.GetComponent<ElliteHealth>();
            BossHealth boss = collision.collider.GetComponent<BossHealth>();

            if (enemy != null)
            {
                if (Time.time - lastDamageTime >= damageInterval)
                {
                    audioSource.Play();
                    float enemyAttack = enemy.currentDamage; // 적의 공격력 가져오기
                    float damageAfterDefense = enemyAttack * (1 - (currentDefense / (100 + currentDefense)));
                    damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10; // 소수점 첫째 자리에서 반올림
                    currentHealth -= damageAfterDefense;
                    _hpBar.value = currentHealth;
                    lastDamageTime = Time.time; // 마지막 데미지 시간 업데이트
                    cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                    ShowDamageText(damageAfterDefense);
                    CheckDeath();
                }
            }
            else if (ellite != null)
            {
                if (Time.time - lastDamageTime >= damageInterval)
                {
                    audioSource.Play();
                    float elliteAttack = ellite.currentDamage; // Ellite의 공격력 가져오기
                    float damageAfterDefense = elliteAttack * (1 - (currentDefense / (100 + currentDefense)));
                    damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10; // 소수점 첫째 자리에서 반올림
                    currentHealth -= damageAfterDefense;
                    _hpBar.value = currentHealth;
                    lastDamageTime = Time.time; // 마지막 데미지 시간 업데이트
                    cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                    ShowDamageText(damageAfterDefense);
                    CheckDeath();
                }
            }
            else if (boss != null)
            {
                if (Time.time - lastDamageTime >= damageInterval)
                {
                    audioSource.Play();
                    float bossAttack = boss.currentDamage; // Boss의 공격력 가져오기
                    float damageAfterDefense = bossAttack * (1 - (currentDefense / (100 + currentDefense)));
                    damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10; // 소수점 첫째 자리에서 반올림
                    currentHealth -= damageAfterDefense;
                    _hpBar.value = currentHealth;
                    lastDamageTime = Time.time; // 마지막 데미지 시간 업데이트
                    cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                    ShowDamageText(damageAfterDefense);
                    CheckDeath();
                }
            }
            isDamage = false; // Damage 처리 후 다시 false로 설정
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isDamage)
        {
            isDamage = true;

            HitBox hitBox = other.GetComponent<HitBox>();
            HitBox2 hitBox2 = other.GetComponent<HitBox2>();
            HitBoxrange hitBoxrange = other.GetComponent<HitBoxrange>();

            float damageAfterDefense;

            if (hitBox != null)
            {
                audioSource.Play();
                float hitAttack = hitBox.attackdamage;
                damageAfterDefense = hitAttack * (1 - (currentDefense / (100 + currentDefense)));
                damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10; // 소수점 첫째 자리에서 반올림
                currentHealth -= damageAfterDefense;
                _hpBar.value = currentHealth;
                ShowDamageText(damageAfterDefense);
                cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                CheckDeath();
            }
            else if (hitBox2 != null)
            {
                audioSource.Play();
                float hitAttack2 = hitBox2.attackdamage;
                damageAfterDefense = hitAttack2 * (1 - (currentDefense / (100 + currentDefense)));
                damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10; // 소수점 첫째 자리에서 반올림
                currentHealth -= damageAfterDefense;
                _hpBar.value = currentHealth;
                ShowDamageText(damageAfterDefense);
                cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                CheckDeath();
            }
            else if (hitBoxrange != null)
            {
                audioSource.Play();
                float hitAttack3 = hitBoxrange.attackdamage + 60;
                damageAfterDefense = hitAttack3 * (1 - (currentDefense / (100 + currentDefense)));
                damageAfterDefense = Mathf.Round(damageAfterDefense * 10) / 10; // 소수점 첫째 자리에서 반올림
                currentHealth -= damageAfterDefense;
                _hpBar.value = currentHealth;
                ShowDamageText(damageAfterDefense);
                cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                CheckDeath();
            }

            isDamage = false;
        }
    }




    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            if (collider != null)
            {
                collider.enabled = false;
            }
            // Death 처리 로직 추가
            if (_hpBar == null)
            {
                Destroy(_hpBar);
            }
            Destroy(rawImage);
            if (exp == null)
            {
                Destroy(exp);
            }
            
        }
        else
        {
            isHit = true;
            StartCoroutine(ResetIsHitAfterDelay(0.5f));
        }
    }

    public void ShowDamageText(float damage)
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

    private IEnumerator ResetIsHitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isHit = false; // 지연 후에 isHit를 false로 설정
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            isHit = false; // 적과의 충돌이 끝나면 isHit를 false로 설정
        }
    }
    private void OnTriggerExit(Collider Other)
    {
        if (Other.CompareTag("Enemy"))
        {
            isHit = false; // 적과의 충돌이 끝나면 isHit를 false로 설정
        }
    }
}
