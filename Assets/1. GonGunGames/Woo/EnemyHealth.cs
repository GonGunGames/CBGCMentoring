using UnityEngine;
using AllUnits;
using UnityEngine.UI;

public class EnemyHealth : Unit
{
    [SerializeField] private Slider e_hpBar;
    public bool isDead { get; private set; } = false;
    private bool isHit = false;

    private void Awake()
    {
        maxHealth = 50;
        SetMaxHealth(maxHealth);
    }

    protected override void Start()
    {
        base.Start();
        damage = 50f; // 적의 공격력을 50으로 설정
    }

    private void SetMaxHealth(float hp)
    {
        e_hpBar.maxValue = hp;
        e_hpBar.value = hp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet") && !isDamage)
        {
            isDamage = true;
            // weapon의 damage 값을 가져오기 위해 PlayerWeapon 스크립트를 참조합니다.
            Weaponbullet bullet = collision.collider.GetComponent<Weaponbullet>();
            if (bullet != null)
            {
                float bulletDamage = bullet.damage; // 총알의 공격력 가져오기
                currentHealth -= bulletDamage;
                e_hpBar.value = currentHealth;

                if (currentHealth <= 0)
                {
                    isDead = true;
                    // 적이 죽었을 때 추가 로직 처리 (예: 애니메이션, 소멸 등)
                }
                else
                {
                    isHit = true; // 적과 충돌 시 isHit를 true로 설정
                    // 피격 시 추가 로직 처리 (예: 애니메이션, 효과 등)
                }
            }
            isDamage = false; // Damage 처리 후 다시 false로 설정
        }
    }
}