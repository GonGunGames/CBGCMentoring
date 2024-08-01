using UnityEngine;
using System;

public class Ellite : MonoBehaviour
{
    public static event Action OnElliteDestroyed;
    public bool isChoose;
    private ChooseManager chooseManager;

    void Start()
    {
        isChoose = false;
        chooseManager = FindObjectOfType<ChooseManager>();
        if (chooseManager == null)
        {
            Debug.LogError("ChooseManager를 찾을 수 없습니다.");
        }
    }

    private void OnDestroy()
    {
        // 보스가 파괴될 때 이벤트를 발생시킵니다.
        if (OnElliteDestroyed != null)
        {
            OnElliteDestroyed.Invoke();
            ChooseUp();
        }
    }

    private void ChooseUp()
    {
        isChoose = true;
        Time.timeScale = 0f;
        if (chooseManager != null)
        {
            chooseManager.SetOnChooseOptionsClosedCallback(OnChooseOptionsClosed);
            chooseManager.ShowChooseOptions();
        }
    }

    private void OnChooseOptionsClosed()
    {
        isChoose = false;
        Time.timeScale = 1f; // 게임 재개
    }
}