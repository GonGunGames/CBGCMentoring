using UnityEngine;

namespace AllUnits
{
    public class Unit : MonoBehaviour
    {
        // 플레이어와 적 유닛이 공통으로 사용할 변수
        [SerializeField] internal float maxHealth = 50f;
        [SerializeField] internal float currentHealth;
        [SerializeField] internal float damage = 5f;
        [SerializeField] protected bool isDamage = false;
        // 자식 클래스들도 사용될 수 있도록
        virtual protected void Start()
        {
            currentHealth = maxHealth;
        }
    }
}