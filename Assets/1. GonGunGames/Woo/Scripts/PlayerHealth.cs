using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;
using UnityEngine.UI;
using System;
using UnityEngine.Experimental.GlobalIllumination;
using System.Runtime.Serialization;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;

    bool isDamage;


    private Rigidbody rb;
    private Animator myAnim;
    [SerializeField] private Slider _hpBar;
    public bool isDead { get; private set; } = false;
    public bool isHit = false;
    public float deathTime;
    private float lastDamageTime = 0f; // 마지막 데미지 시간
    private float damageInterval = 1f; // 데미지 간격 (초)

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = DataBase.Instance.playerData.maxHealth;
        SetMaxHealth(currentHealth);
    }

    public void SetMaxHealth(float hp)
    {
        _hpBar.maxValue = hp;
        _hpBar.value = hp;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && !isDamage)
        {
            isDamage = true;
            // Enemy의 damage 값을 가져오기 위해 EnemyHealth 스크립트를 참조합니다.
            EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                // 현재 시간과 마지막 데미지 시간 비교
                if (Time.time - lastDamageTime >= damageInterval)
                {
                    float enemyAttack = enemy.currentDamage; // 적의 공격력 가져오기
                    currentHealth -= enemyAttack;
                    _hpBar.value = currentHealth;
                    lastDamageTime = Time.time; // 마지막 데미지 시간 업데이트

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
            isDamage = true;
            HitBox hitBox = other.GetComponent<HitBox>();
            if (hitBox != null)
            {
                Debug.Log("EnemyHealth와 HitBox 컴포넌트를 찾음");
                float hitAttack = hitBox.attackdamage;
                currentHealth -= hitAttack;
                _hpBar.value = currentHealth;
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