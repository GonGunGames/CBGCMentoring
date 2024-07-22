using UnityEngine;



public class PlayerStats : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToLevelUp = 100; // 레벨업에 필요한 경험치


    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // 경험치 획득 메서드
    public void GainExp(int exp)
    {
        currentExp += exp;
        // 레벨업 체크
        CheckLevelUp();
    }

    // 레벨업 체크 메서드
    private void CheckLevelUp()
    {
        if (currentExp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    // 레벨업 메서드
    private void LevelUp()
    {
        currentLevel++;
        currentExp -= expToLevelUp;
        expToLevelUp = CalculateNextLevelExp(); // 다음 레벨에 필요한 경험치 계산

        // 레벨업 시 선택지 제공
        levelManager.ShowUpgradeOptions();
    }

    // 다음 레벨에 필요한 경험치 계산
    private int CalculateNextLevelExp()
    {
        // 간단히 다음 레벨이 더 많은 경험치가 필요하다고 가정
        return expToLevelUp * 2;
    }

    // 선택한 옵션에 따라 능력치 업그레이드

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemEx item = other.GetComponent<ItemEx>();
            if (item != null)
            {
                GainExp(item.expAmount); // 아이템에서 정의된 경험치 양을 플레이어의 경험치에 추가
                Debug.Log("아이템을 획득하여 경험치가 증가했습니다.");
                Destroy(other.gameObject); // 충돌한 아이템 오브젝트 파괴
            }
        }
    }

}
