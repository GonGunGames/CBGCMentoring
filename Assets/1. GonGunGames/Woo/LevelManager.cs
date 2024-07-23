using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public Weapon weapon;
    public Button attackSpeedButton;
    public Button attackDamageButton;
    public Button attackRangeButton;

    private Action onUpgradeOptionsClosed; // 콜백을 저장할 변수

    private void Start()
    {
        // 버튼 클릭 이벤트에 메서드 연결
        attackSpeedButton.onClick.AddListener(() => OnUpgradeButtonClicked(UpgradeOption.AttackSpeed));
        attackDamageButton.onClick.AddListener(() => OnUpgradeButtonClicked(UpgradeOption.AttackDamage));
        attackRangeButton.onClick.AddListener(() => OnUpgradeButtonClicked(UpgradeOption.AttackChance));

        // 처음에는 버튼들을 비활성화
        SetButtonsActive(false);
    }

    private void OnUpgradeButtonClicked(UpgradeOption option)
    {
        weapon.UpgradeStat(option);
        SetButtonsActive(false); // 선택 후 버튼들을 다시 비활성화
        OnUpgradeOptionsClosed(); // 콜백 호출
    }

    public void ShowUpgradeOptions()
    {
        Debug.Log("레벨 업! 능력을 업그레이드할 옵션을 선택하세요:");
        SetButtonsActive(true); // 버튼들을 활성화
    }

    public void SetOnUpgradeOptionsClosedCallback(Action callback)
    {
        onUpgradeOptionsClosed = callback; // 콜백 저장
    }

    private void OnUpgradeOptionsClosed()
    {
        if (onUpgradeOptionsClosed != null)
        {
            onUpgradeOptionsClosed.Invoke(); // 콜백 호출
        }
    }

    private void SetButtonsActive(bool isActive)
    {
        attackSpeedButton.gameObject.SetActive(isActive);
        attackDamageButton.gameObject.SetActive(isActive);
        attackRangeButton.gameObject.SetActive(isActive);
    }
}