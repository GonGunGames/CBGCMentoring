using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour
{
    public PlayerExp playerExp;
    public GameObject Ui;
    public Weapon weapon;
    public Shotgun shotgun;
    public Button attackSpeedButton;
    public Button attackDamageButton;
    public Button attackRangeButton;
    public RawImage rawImage;
    public Text leveltext;
    public Text noticetext;

    private Action onUpgradeOptionsClosed; // 콜백을 저장할 변수

    private void Start()
    {
        Ui.SetActive(false);
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
        shotgun.UpgradeStat(option);
        SetButtonsActive(false); // 선택 후 버튼들을 다시 비활성화
        Ui.SetActive(false );
        OnUpgradeOptionsClosed(); // 콜백 호출
    }

    public void ShowUpgradeOptions()
    {
        Debug.Log("레벨 업! 능력을 업그레이드할 옵션을 선택하세요:");
        Ui.SetActive(true);
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
        rawImage.gameObject.SetActive(isActive);
        leveltext.gameObject.SetActive(isActive);
        noticetext.gameObject.SetActive(isActive);
    }
}