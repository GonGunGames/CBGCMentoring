using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum ChooseOption
{
    Heal,
    Magnetic,
    Blank,
    Gold
}

public class ChooseManager : MonoBehaviour
{
    public Button heal;
    public Button blank;
    public Button magnetic;
    public Button gold;
    public Text chooseText;
    public RawImage rawImage;

    private Action onChooseOptionsClosed; // 콜백을 저장할 변수

    private void Start()
    {
        // 버튼 클릭 이벤트에 메서드 연결
        heal.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Heal));
        blank.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Blank));
        magnetic.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Magnetic));
        gold.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Gold));

        // 처음에는 버튼들을 비활성화
        SetButtonsActive(false);
    }

    private void OnChooseButtonClicked(ChooseOption option)
    {
        SetButtonsActive(false); // 선택 후 버튼들을 다시 비활성화
        OnChooseOptionsClosed(); // 콜백 호출
    }

    public void ShowChooseOptions()
    {
        Debug.Log("레벨 업! 능력을 업그레이드할 옵션을 선택하세요:");
        SetButtonsActive(true); // 버튼들을 활성화
    }

    public void SetOnChooseOptionsClosedCallback(Action callback)
    {
        onChooseOptionsClosed = callback; // 콜백 저장
    }

    private void OnChooseOptionsClosed()
    {
        if (onChooseOptionsClosed != null)
        {
            onChooseOptionsClosed.Invoke(); // 콜백 호출
        }
    }

    private void SetButtonsActive(bool isActive)
    {
        heal.gameObject.SetActive(isActive);
        blank.gameObject.SetActive(isActive);
        magnetic.gameObject.SetActive(isActive);
        gold.gameObject.SetActive(isActive);
        rawImage.gameObject.SetActive(isActive);
        chooseText.gameObject.SetActive(isActive);
    }
}
