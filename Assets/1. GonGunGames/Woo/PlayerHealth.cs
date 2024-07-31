using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;
using UnityEngine.UI;

public class PlayerHealth : Unit
{
    private Rigidbody rb;
    private Animator myAnim;
    [SerializeField] private Slider _hpBar;
    public bool isDead { get; private set; } = false;
    public bool isHit = false;

    private void Awake()
    {
        maxHealth = 100;
        SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
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
            // Enemy의 damage 값을 가져오기 위해 Enemy 스크립트를 참조합니다.
            EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                Debug.Log("dhdlsghks");
                float enemyAttack = enemy.damage; // 적의 공격력 가져오기
                currentHealth -= enemyAttack;
                _hpBar.value = currentHealth;
                if (currentHealth <= 0)
                {
                    isDead = true;
                }
                else
                {
                    isHit = true; // 적과 충돌 시 isHit를 true로 설정
                    StartCoroutine(ResetIsHitAfterDelay(3f)); // 1초 후에 isHit를 false로 설정하는 코루틴 시작
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