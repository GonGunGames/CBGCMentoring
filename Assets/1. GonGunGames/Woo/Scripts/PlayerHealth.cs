using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;
using UnityEngine.UI;
using System;
using UnityEngine.Experimental.GlobalIllumination;
using System.Runtime.Serialization;
using UnityEditor.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    bool isDamage;
    private CameraFollow cameraFollow;  // CameraFollow 스크립트 참조

    private Rigidbody rb;
    private Animator myAnim;
    private Renderer playerRenderer;  // 플레이어의 Renderer 컴포넌트 참조
    [SerializeField] private Slider _hpBar;
    public bool isDead { get; private set; } = false;
    public bool isHit = false;
    public float deathTime;
    private float lastDamageTime = 0f; // 마지막 데미지 시간
    private float damageInterval = 1f; // 데미지 간격 (초)
    public GameObject damageTextPrefab;  // 데미지 텍스트 프리팹
    public Transform damageTextSpawnPoint;  // 데미지 텍스트가 생성될 위치
    public AudioSource audioSource;

    private Color originalColor;  // 원래 색상 저장
    public float flashDuration = 0.1f;  // 깜빡임 지속 시간

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        playerRenderer = GetComponent<Renderer>(); // Renderer 컴포넌트 가져오기
        originalColor = playerRenderer.material.color; // 원래 색상 저장
    }

    private void Start()
    {
        //currentHealth = DataBase.Instance.playerData.maxHealth;
        currentHealth = DataBase.Instance.playerData.maxHealth;
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
        currentHealth = DataBase.Instance.playerData.maxHealth; // 체력을 최대 값으로 설정
        Debug.Log("Player healed to max health.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && !isDamage)
        {
            isDamage = true;
            // Enemy의 damage 값을 가져오기 위해 EnemyHealth 스크립트를 참조합니다.
            EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
            audioSource.Play();
            if (enemy != null)
            {
                // 현재 시간과 마지막 데미지 시간 비교
                if (Time.time - lastDamageTime >= damageInterval)
                {
                    float enemyAttack = enemy.currentDamage; // 적의 공격력 가져오기
                    currentHealth -= enemyAttack;
                    _hpBar.value = currentHealth;
                    lastDamageTime = Time.time; // 마지막 데미지 시간 업데이트
                    cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                    ShowDamageText(enemyAttack);
                    StartCoroutine(FlashRed()); // 플레이어 색상 깜빡임 시작
                    if (currentHealth <= 0)
                    {
                        isDead = true;
                        // Death 처리 로직 추가
                    }
                    else
                    {
                        isHit = true; // 적과 충돌 시 isHit를 true로 설정
                        StartCoroutine(ResetIsHitAfterDelay(3f)); // 3초 후에 isHit를 false로 설정하는 코루틴 시작
                    }
                }
            }

            isDamage = false; // Damage 처리 후 다시 false로 설정
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isDamage)
        {
            audioSource.Play();
            isDamage = true;
            HitBox hitBox = other.GetComponent<HitBox>();
            if (hitBox != null)
            {
                Debug.Log("EnemyHealth와 HitBox 컴포넌트를 찾음");
                float hitAttack = hitBox.attackdamage;
                currentHealth -= hitAttack;
                _hpBar.value = currentHealth;
                ShowDamageText(hitAttack);
                cameraFollow.TriggerShake(); // 카메라 흔들림 트리거
                StartCoroutine(FlashRed()); // 플레이어 색상 깜빡임 시작
                if (currentHealth <= 0)
                {
                    isDead = true;
                }
                else
                {
                    isHit = true;
                    StartCoroutine(ResetIsHitAfterDelay(3f));
                }
            }
            isDamage = false;

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
    private IEnumerator FlashRed()
    {
        playerRenderer.material.color = Color.red; // 색상을 빨간색으로 변경
        yield return new WaitForSeconds(flashDuration); // 일정 시간 대기
        playerRenderer.material.color = originalColor; // 원래 색상으로 복귀
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
}
