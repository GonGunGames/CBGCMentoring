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
    public GameObject Ui;
    private EnemyHealth enemyHealth;
    private ElliteHealth elliteHealth;
    private BossHealth bossHealth;
    private Action onChooseOptionsClosed; // 콜백을 저장할 변수
    private PlayerHealth playerHealth;
    private void Start()
    {
        Ui.SetActive(false);
        // 버튼 클릭 이벤트에 메서드 연결
        heal.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Heal));
        blank.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Blank));
        magnetic.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Magnetic));
        gold.onClick.AddListener(() => OnChooseButtonClicked(ChooseOption.Gold));

        // 처음에는 버튼들을 비활성화
        SetButtonsActive(false);

        // PlayerHealth 컴포넌트를 찾습니다.
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth를 찾을 수 없습니다.");
        }
    }

    private void OnChooseButtonClicked(ChooseOption option)
    {
        switch (option)
        {
            case ChooseOption.Heal:
                HealPlayer();
                break;
            case ChooseOption.Magnetic:
                ApplyMagneticEffect();
                break;
            case ChooseOption.Blank:
                ApplyBlankEffect();
                break;
            case ChooseOption.Gold:

                // Gold 관련 로직을 여기에 추가하세요.
                break;
        }

        SetButtonsActive(false); // 선택 후 버튼들을 다시 비활성화
        Ui.SetActive(false);
        OnChooseOptionsClosed(); // 콜백 호출
    }

    private void HealPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.HealToMax(); // PlayerHealth의 체력을 최대 값으로 설정
        }
    }

    public void ShowChooseOptions()
    {
        Debug.Log("레벨 업! 능력을 업그레이드할 옵션을 선택하세요:");
        Ui.SetActive(true);
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
    private void ApplyMagneticEffect()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player를 찾을 수 없습니다.");
            return;
        }

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            StartCoroutine(MoveItemToPlayer(item, player));
        }
    }

    private IEnumerator MoveItemToPlayer(GameObject item, GameObject player)
    {
        while (item != null && player != null)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, player.transform.position, 20f * Time.deltaTime);
            yield return null;
        }
    }
    private void ApplyBlankEffect()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            StartCoroutine(FreezeEnemyForSeconds(enemy, 3f)); // 3초 동안 적을 얼립니다.
        }
    }

    private IEnumerator FreezeEnemyForSeconds(GameObject enemy, float seconds)
    {
        if (enemy == null)
            yield break;

        // 각 클래스의 movementSpeed를 가져옵니다.
        CommonMob commonMob = enemy.GetComponent<CommonMob>();
        CommonMobB commonMobB = enemy.GetComponent<CommonMobB>();
        CommonMobN commonMobN = enemy.GetComponent<CommonMobN>();

        // 원래 속도를 저장합니다.
        float originalSpeed = 0f;
        float originalSpeedB = 0f;
        float originalSpeedN = 0f;

        if (commonMob != null)
        {
            originalSpeed = commonMob.moveSpeed;
            commonMob.moveSpeed = 0f; // 속도를 0으로 설정하여 멈춤
            commonMob.SetState(FSMState.Hit); // Hit 상태로 전환
            yield return new WaitForSeconds(0.1f); // Hit 애니메이션을 잠깐 보여줌
            commonMob.SetState(FSMState.Idle); // 바로 Idle 상태로 전환
        }
        if (commonMobB != null)
        {
            originalSpeedB = commonMobB.moveSpeed;
            commonMobB.moveSpeed = 0f; // 속도를 0으로 설정하여 멈춤
            commonMobB.SetState(FSMState.Hit); // Hit 상태로 전환
            yield return new WaitForSeconds(0.1f); // Hit 애니메이션을 잠깐 보여줌
            commonMobB.SetState(FSMState.Idle); // 바로 Idle 상태로 전환
        }
        if (commonMobN != null)
        {
            originalSpeedN = commonMobN.moveSpeed;
            commonMobN.moveSpeed = 0f; // 속도를 0으로 설정하여 멈춤
            commonMobN.SetState(FSMState.Hit); // Hit 상태로 전환
            yield return new WaitForSeconds(0.1f); // Hit 애니메이션을 잠깐 보여줌
            commonMobN.SetState(FSMState.Idle); // 바로 Idle 상태로 전환
        }

        // Hit 상태에서 Idle로 전환된 후, 전체 지속 시간 동안 대기
        yield return new WaitForSeconds(seconds - 0.1f);

        // 원래 속도로 복구
        if (commonMob != null)
        {
            commonMob.moveSpeed = originalSpeed;
            commonMob.SetState(FSMState.Move); // 바로 Idle 상태로 전
        }
        if (commonMobB != null)
        {
            commonMobB.moveSpeed = originalSpeedB;
            commonMobB.SetState(FSMState.Move); // 바로 Idle 상태로 전
        }
        if (commonMobN != null)
        {
            commonMobN.moveSpeed = originalSpeedN;
            commonMobN.SetState(FSMState.Move); // 바로 Idle 상태로 전
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