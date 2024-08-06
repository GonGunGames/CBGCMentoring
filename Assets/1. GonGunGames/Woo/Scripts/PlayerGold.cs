using UnityEngine;
using UnityEngine.UI;

public class PlayerGold : MonoBehaviour
{
    public int totalGold; // 플레이어의 총 골드
    public Text goldText; // 골드를 표시할 UI 텍스트

    private void Start()
    {
        // 초기화: 데이터베이스에서 총 골드를 가져오거나 기본값으로 설정
        totalGold = DataBase.Instance.playerData.gold;

        // UI 텍스트 초기화
        UpdateGoldText();
    }

    // 골드를 증가시키는 메서드
    public void AddGold(int amount)
    {
        totalGold += amount;
        UpdateGoldText(); // 골드 UI 업데이트
    }

    // UI 텍스트를 골드 값으로 업데이트하는 메서드
    private void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + totalGold.ToString();
        }
        else
        {
            Debug.LogError("goldText is not assigned in PlayerGold script.");
        }
    }
}