using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentLevel;
    public double currentExp;
    public double expToLevelUp; // 레벨업에 필요한 경험치

    private LevelManager levelManager;
    private bool isLevelingUp = false; // 레벨업 중인지 여부를 체크하는 변수

    private void Start()
    {
        currentLevel = DataBase.Instance.playerData.currentLevel;
        currentExp = DataBase.Instance.playerData.currentExp;
        expToLevelUp = DataBase.Instance.playerData.expToLevelUp;
        levelManager = FindObjectOfType<LevelManager>();
    }

    // 경험치 획득 메서드
    public void GainExp(int exp)
    {
        if (!isLevelingUp) // 레벨업 중일 때는 경험치를 추가하지 않음
        {
            currentExp += exp;
            // 레벨업 체크
            CheckLevelUp();
        }
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
        isLevelingUp = true; // 레벨업 시작
        Time.timeScale = 0f; // 게임 일시 정지

        currentLevel++;
        currentExp -= expToLevelUp;
        expToLevelUp = CalculateNextLevelExp(); // 다음 레벨에 필요한 경험치 계산

        // 레벨업 시 선택지 제공
        levelManager.SetOnUpgradeOptionsClosedCallback(OnUpgradeOptionsClosed);
        levelManager.ShowUpgradeOptions();
    }

    private void OnUpgradeOptionsClosed()
    {
        isLevelingUp = false; // 레벨업 완료
        Time.timeScale = 1f; // 게임 재개
    }

    // 다음 레벨에 필요한 경험치 계산
    private double CalculateNextLevelExp()
    {
        // 간단히 다음 레벨이 더 많은 경험치가 필요하다고 가정
        return expToLevelUp * 1.5;
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
                Destroy(other.gameObject); // 충돌한 아이템 오브젝트 파괴
            }
        }
    }
}