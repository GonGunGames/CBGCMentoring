using UnityEngine;
using UnityEngine.UI;
using AllUnits;

public class EnemyHealth : Unit
{
    [SerializeField] private Slider e_hpBar;
    public bool isDead { get; private set; } = false;
    private bool isHit = false;
    private CommonMob commonMob;
    public PlayerStats playerStats;
    public Weapon weapon;

    private void Awake()
    {
        maxHealth = 50;
        SetMaxHealth(maxHealth);
        commonMob = GetComponent<CommonMob>(); // CommonMob 컴포넌트를 가져옵니다.
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

            Weaponbullet bullet = collision.collider.GetComponent<Weaponbullet>();
            Weaponbullet2 bullet2 = collision.collider.GetComponent<Weaponbullet2>();

            if (bullet2 != null)
            {
                // Weaponbullet2의 폭발 범위 내의 적에게 데미지를 입히는 메서드를 호출합니다.
                float bulletDamage = weapon.attackDamage;
                bullet2.NotifyExplosion();
            }
            else if (bullet != null)
            {
                // Weaponbullet의 데미지를 처리합니다.
                float bulletDamage = weapon.attackDamage;
                ApplyDamage(bulletDamage);
            }

            isDamage = false; // Damage 처리 후 다시 false로 설정
        }
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        e_hpBar.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            // 적 사망 시 추가 로직 (예: 애니메이션, 아이템 드랍 등) 처리
        }

        isHit = true; // 적과 충돌 시 isHit를 true로 설정
        commonMob.SetState(FSMState.Hit); // 피격 시 Hit 상태로 전환
    }
}