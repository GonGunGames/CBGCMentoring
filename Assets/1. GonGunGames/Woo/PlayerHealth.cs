using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;

public class PlayerHealth : Unit
{
    private Rigidbody rb;
    private Animator myAnim;
    public bool isDead { get; private set; } = false;
    public bool isHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") && !isDamage)
        {
            isDamage = true;
            // Enemy의 damage 값을 가져오기 위해 Enemy 스크립트를 참조합니다.
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float enemyAttack = enemy.damage; // 적의 공격력 가져오기
                currentHealth -= enemyAttack;

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