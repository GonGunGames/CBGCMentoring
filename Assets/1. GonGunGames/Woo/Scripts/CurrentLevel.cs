using UnityEngine;
using UnityEngine.UI;

public class CurrentLevel : MonoBehaviour
{
    private PlayerExp playerExp;
    public Text levelText; // UI 텍스트 컴포넌트를 참조하기 위한 변수

    private void Start()
    {
        // PlayerExp 컴포넌트를 찾습니다.
        playerExp = FindObjectOfType<PlayerExp>();

        // 초기 텍스트 설정
        UpdateLevelText();
    }

    // Update is called once per frame
    void Update()
    {
        // 레벨 업데이트
        UpdateLevelText();
    }

    // 레벨 텍스트를 업데이트하는 메서드
    private void UpdateLevelText()
    {
        if (playerExp != null && levelText != null)
        {
            levelText.text = "Lv." + playerExp.currentLevel;
        }
    }
}