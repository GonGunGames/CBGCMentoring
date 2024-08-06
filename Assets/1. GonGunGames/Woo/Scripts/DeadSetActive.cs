using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class DeadSetActive : MonoBehaviour
{
    public PlayerHealth health;
    public Button retrybt;
    public Button loselobby;
    public Button next;
    public RawImage total;
    public RawImage choose;
    public Button clearlobby;
    public Button heal;
    public Button gold;
    public Button blank;
    public Button magnetic;
    public Text chooseText;
    public Text clearText;
    public Text totalElapsedTimeText; // 시작부터 죽을 때까지의 경과 시간을 표시할 UI Text
    public Text elapsedSinceDeathText; // 죽을 때까지의 경과 시간을 표시할 UI Text
    public Text bossDefeatedText; // 보스 처치 시간을 표시할 UI Text (새로 추가)

    private float startTime;
    private float deathTime;
    private float bossDefeatTime; // 보스 처치 시간을 기록합니다.
    private float elliteDefeatTime;
    private bool isPlayerDead = false;
    private bool isBossDead = false;
    private bool isElliteDead = false;

    private void Start()
    {
        // 초기에 필요한 UI 요소를 설정합니다.
        retrybt.gameObject.SetActive(false);
        loselobby.gameObject.SetActive(false);
        total.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        clearlobby.gameObject.SetActive(false);
        choose.gameObject.SetActive(false);
        clearText.gameObject.SetActive(false);
        totalElapsedTimeText.gameObject.SetActive(true);
        elapsedSinceDeathText.gameObject.SetActive(false); // 처음에는 죽을 때까지의 시간은 숨깁니다.
        bossDefeatedText.gameObject.SetActive(false); // 처음에는 보스 처치 시간을 숨깁니다.

        // 게임 시작 시간을 기록합니다.
        startTime = Time.time;

        // 보스 파괴 이벤트에 대한 리스너를 추가합니다.
        Boss.OnBossDestroyed += HandleBossDestroyed;
        Ellite.OnElliteDestroyed += HandleElliteDestroyed;
    }

    private void OnDestroy()
    {
        // 보스 파괴 이벤트에 대한 리스너를 제거합니다.
        Boss.OnBossDestroyed -= HandleBossDestroyed;
        Ellite.OnElliteDestroyed -= HandleElliteDestroyed;
    }

    void Update()
    {
        // health가 null이 아니고, 캐릭터가 죽었을 때
        if (health != null && health.isDead && !isPlayerDead)
        {
            isPlayerDead = true; // 캐릭터가 죽었다는 상태를 기록합니다.
            deathTime = Time.time; // 죽은 시간을 기록합니다.

            // UI 요소를 활성화/비활성화 처리합니다.
            retrybt.gameObject.SetActive(true);
            loselobby.gameObject.SetActive(true);
            total.gameObject.SetActive(true);
            totalElapsedTimeText.gameObject.SetActive(false); // 시작부터 죽을 때까지의 시간은 숨깁니다.
            elapsedSinceDeathText.gameObject.SetActive(true); // 죽을 때까지의 시간을 표시합니다.

            // 죽을 때까지의 경과 시간을 계산하여 표시합니다.
            float elapsedTime = deathTime - startTime;
            elapsedSinceDeathText.text = FormatTime(elapsedTime);
        }
        else if (!isPlayerDead)
        {
            // 플레이어가 살아 있는 동안의 전체 경과 시간을 계산하여 표시합니다.
            float totalElapsedTime = Time.time - startTime;
            totalElapsedTimeText.text = FormatTime(totalElapsedTime);
        }
    }

    void HandleElliteDestroyed()
    {
        if (!isElliteDead)
        {
            isElliteDead = true;
            elliteDefeatTime = Time.time;

            // UI 요소를 활성화/비활성화 처리합니다.
            heal.gameObject.SetActive(true);
            gold.gameObject.SetActive(true);
            blank.gameObject.SetActive(true);
            choose.gameObject.SetActive(true);
            magnetic.gameObject.SetActive(true);
            chooseText.gameObject.SetActive(true);
        }
    }

    void HandleBossDestroyed()
    {
        if (!isBossDead)
        {
            isBossDead = true; // 보스가 죽었다는 상태를 기록합니다.
            bossDefeatTime = Time.time; // 보스가 죽은 시간을 기록합니다.

            // UI 요소를 활성화/비활성화 처리합니다.
            retrybt.gameObject.SetActive(true);
            total.gameObject.SetActive(true);
            next.gameObject.SetActive(true);
            clearlobby.gameObject.SetActive(true);
            clearText.gameObject.SetActive(true);
            totalElapsedTimeText.gameObject.SetActive(false); // 시작부터 죽을 때까지의 시간은 숨깁니다.
            bossDefeatedText.gameObject.SetActive(true); // 보스 처치 시간을 표시합니다.

            // 보스 처치까지의 경과 시간을 계산하여 표시합니다.
            float elapsedTime = bossDefeatTime - startTime;
            bossDefeatedText.text = "총 플레이타임 " + FormatTime(elapsedTime);
        }
    }

    string FormatTime(float timeToFormat)
    {
        // 시간을 분과 초로 변환하여 포맷합니다.
        float minutes = Mathf.FloorToInt(timeToFormat / 60);
        float seconds = Mathf.FloorToInt(timeToFormat % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}