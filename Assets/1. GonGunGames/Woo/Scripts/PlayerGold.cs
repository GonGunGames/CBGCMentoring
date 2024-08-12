using UnityEngine;
using UnityEngine.UI;

public class PlayerGold : MonoBehaviour
{
    public int totalGold; // 게임 내에서 관리하는 총 골드
    public Text goldText; // 골드를 표시할 UI 텍스트
    public DataPlayer dataPlayer; // DataPlayer 참조
    private PlayerStat playerStat;
    private void Awake()
    {
        // 게임 시작 시 totalGold를 0으로 초기화
        totalGold = 0;

        // UI 텍스트 초기화
        UpdateGoldText();
    }

    // 골드를 증가시키는 메서드
    public void AddGold(int amount)
    {
        totalGold += amount;

        // DataPlayer의 gold 필드에 추가된 골드를 저장
        if (dataPlayer != null)
        {
            dataPlayer.gold += amount; // 데이터에 골드를 추가

        }
        if (playerStat != null)
        {
            playerStat.playerData.gold += amount;
        }
        UpdateGoldText(); // 골드 UI 업데이트
    }

    // UI 텍스트를 골드 값으로 업데이트하는 메서드
    private void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + totalGold.ToString(); // 게임 내 골드 값 표시
        }
        else
        {
            Debug.LogError("goldText is not assigned in PlayerGold script.");
        }
    }
}
