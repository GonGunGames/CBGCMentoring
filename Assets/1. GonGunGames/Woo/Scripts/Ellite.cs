using UnityEngine;
using System;

public class Ellite : MonoBehaviour
{
    public static event Action OnElliteDestroyed;
    public bool isChoose;
    private ElliteHealth elliteHealth;
    private ChooseManager chooseManager;

    void Start()
    {
        isChoose = false;
        elliteHealth = GetComponent<ElliteHealth>(); // ElliteHealth 컴포넌트 가져오기
        chooseManager = FindObjectOfType<ChooseManager>();
        if (chooseManager == null)
        {
            Debug.LogError("ChooseManager를 찾을 수 없습니다.");
        }
    }

    public void DeadEllite()
    {
        if (elliteHealth != null && elliteHealth.isDead)
        {
            OnElliteDestroyed?.Invoke();
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