using UnityEngine;
using UnityEngine.UI;
using AllUnits;

public class EnemyHealth : MonoBehaviour
{
    public int currentId; // 인스펙터에서 설정할 수 있도록 public으로 설정
    public float currentDamage;
    public float currentHealth;

    [SerializeField] private Slider e_hpBar;
    public bool isDead { get; private set; } = false;
    public bool isHit = false;
    private CommonMob commonMob;
    private CommonMobN commonMobN;
    private CommonMobB commonMobB;
    public Weapon weapon;

    private void Start()
    {
        // 인스펙터에서 설정된 currentId를 사용합니다.
        // 또는 여기서 직접 설정할 수 있습니다.
        // 예: currentId = 1;

        // 특정 ID에 해당하는 적 정보를 가져옵니다.
        EnemyInfo enemyInfo = DataBase.Instance.GetEnemyInfoById(currentId);

        if (enemyInfo != null)
        {
            currentId = enemyInfo.id;
            currentHealth = enemyInfo.maxHealth;
            currentDamage = enemyInfo.damage;
            SetMaxHealth(currentHealth);
        }
        else
        {
            Debug.LogError("Enemy with ID " + currentId + " not found.");
        }

        commonMob = GetComponent<CommonMob>(); // CommonMob 컴포넌트를 가져옵니다.
    }




    private void SetMaxHealth(float hp)
    {
        e_hpBar.maxValue = hp;
        e_hpBar.value = hp;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log("Damage ON");
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
        }
    }

    public void ApplyDamage(float damage)
    {

        currentHealth -= weapon.attackDamage;
        e_hpBar.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            // 적 사망 시 추가 로직 (예: 애니메이션, 아이템 드랍 등) 처리
        }
        isHit = true; // 적과 충돌 시 isHit를 true로 설정
        commonMob?.SetState(FSMState.Hit); // CommonMob의 Hit 상태로 전환
        commonMobN?.SetState(FSMState.Hit); // CommonMobN의 Hit 상태로 전환
        commonMobB?.SetState(FSMState.Hit);
        // 피격 시 추가 로직 처리 (예: 애니메이션, 효과 등)

    }
}