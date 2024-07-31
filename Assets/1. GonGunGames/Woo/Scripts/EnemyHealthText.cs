using UnityEngine;

public class EnemyHealthText : MonoBehaviour
{
    public float attackDamage;
    private WeaponInfo currentWeapon;
    public float currentHealth;
    private DamageTextManager damageTextManager;

    void Start()
    {
        if (currentWeapon != null && currentHealth != null)
        {
            attackDamage = currentWeapon.attackDamage;
        }

        // DamageTextManager 컴포넌트 찾기
        damageTextManager = FindObjectOfType<DamageTextManager>();
    }


    private void TakeDamage(float damage)
    {
        // 데미지 텍스트 표시
        if (damageTextManager != null)
        {
            damageTextManager.ShowDamageText(transform.position, damage);
        }

        // 적의 체력 감소 로직 추가
        // 예: health -= damage;
    }
}