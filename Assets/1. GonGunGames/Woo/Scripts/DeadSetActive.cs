using UnityEngine;
using UnityEngine.UI;

public class DeadSetActive : MonoBehaviour
{
    public PlayerHealth health;
    public PlayerGold playerGold; // PlayerGold 컴포넌트를 추가합니다.
    public BossHealth bossHealth;
    public GameObject dead;
    public GameObject success;
    public GameObject ellite;
    public GameObject total;
    public GameObject totalAlert;
    public Text totalElapsedTimeText; // 시작부터 죽을 때까지의 경과 시간을 표시할 UI Text
    public Text elapsedSinceDeathText; // 죽을 때까지의 경과 시간을 표시할 UI Text
    public Text bossDefeatedText; // 보스 처치 시간을 표시할 UI Text (새로 추가)
    public Text deathCountText; // 총 데스 카운트를 표시할 UI Text
    public Text totalGoldText; // 총 골드를 표시할 UI Text

    private float startTime;
    private float deathTime;
    private float bossDefeatTime; // 보스 처치 시간을 기록합니다.
    private float elliteDefeatTime;
    private bool isPlayerDead = false;
    private bool isBossDead = false;
    private bool isElliteDead = false;


    private void Awake()
    {
        ResumeGame();
    }
    private void Start()
    {
        // UI 요소를 초기화합니다.
        dead.gameObject.SetActive(false);
        success.gameObject.SetActive(false);
        total.gameObject.SetActive(false);
        ellite.gameObject.SetActive(false);
        totalAlert.gameObject.SetActive(false);
        totalElapsedTimeText.gameObject.SetActive(true);
        elapsedSinceDeathText.gameObject.SetActive(false); // 처음에는 죽을 때까지의 시간은 숨깁니다.
        bossDefeatedText.gameObject.SetActive(false); // 처음에는 보스 처치 시간을 숨깁니다.
        deathCountText.gameObject.SetActive(false); // 처음에는 데스 카운트를 숨깁니다.
        totalGoldText.gameObject.SetActive(false); // 처음에는 총 골드를 숨깁니다.

        // 게임 시작 시간을 기록합니다.
        startTime = Time.time;

        // PlayerGold 컴포넌트를 찾습니다.
        playerGold = FindObjectOfType<PlayerGold>();
        if (playerGold == null)
        {
            Debug.LogError("PlayerGold component is not found in the scene.");
        }

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
        if (health != null && health.isDead && !isPlayerDead)
        {
            isPlayerDead = true; // 캐릭터가 죽었다는 상태를 기록합니다.
            deathTime = Time.time; // 죽은 시간을 기록합니다.

            // UI 요소를 활성화/비활성화 처리합니다.
            dead.gameObject.SetActive(true);
            total.gameObject.SetActive(true);
            totalAlert.gameObject.SetActive(true);
            totalElapsedTimeText.gameObject.SetActive(false); // 시작부터 죽을 때까지의 시간은 숨깁니다.
            elapsedSinceDeathText.gameObject.SetActive(true); // 죽을 때까지의 시간을 표시합니다.
            deathCountText.gameObject.SetActive(true); // 데스 카운트 표시
            totalGoldText.gameObject.SetActive(true); // 총 골드 표시

            // 죽을 때까지의 경과 시간을 계산하여 표시합니다.
            float elapsedTime = deathTime - startTime;
            elapsedSinceDeathText.text = FormatTime(elapsedTime);

            // 총 데스 카운트를 UI에 표시합니다.
            deathCountText.text = "" + DeathCount.Instance.GetDeathCount().ToString();

            // 총 골드를 UI에 표시합니다.
            if (playerGold != null)
            {
                totalGoldText.text = "" + playerGold.totalGold.ToString();
            }

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
            total.gameObject.SetActive(true);
            elliteDefeatTime = Time.time;

            // UI 요소를 활성화/비활성화 처리합니다.
            ellite.gameObject.SetActive(true);
        }
    }

    void HandleBossDestroyed()
    {
        if (!isBossDead)
        {
            isBossDead = true;
            bossDefeatTime = Time.time;

            total.gameObject.SetActive(true);
            success.gameObject.SetActive(true);
            totalElapsedTimeText.gameObject.SetActive(false);
            bossDefeatedText.gameObject.SetActive(true);
            totalAlert.gameObject.SetActive(true);
            deathCountText.gameObject.SetActive(true); // 데스 카운트 표시
            totalGoldText.gameObject.SetActive(true); // 총 골드 표시
            float elapsedTime = bossDefeatTime - startTime;
            bossDefeatedText.text = "총 플레이타임 " + FormatTime(elapsedTime);

            // 총 데스 카운트를 UI에 표시합니다.
            if (DeathCount.Instance != null)
            {
                int deathCount = DeathCount.Instance.GetDeathCount();
                deathCountText.text = "" + deathCount.ToString();
            }

            // 총 골드를 UI에 표시합니다.
            if (playerGold != null)
            {
                int totalGold = playerGold.totalGold;
                totalGoldText.text = "" + totalGold.ToString();
            }
            PauseGame();
        }
    }
    // 게임을 정지시키는 메서드
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("게임이 정지되었습니다.");
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("게임이 정지되었습니다.");
    }
    string FormatTime(float timeToFormat)
    {
        // 시간을 분과 초로 변환하여 포맷합니다.
        float minutes = Mathf.FloorToInt(timeToFormat / 60);
        float seconds = Mathf.FloorToInt(timeToFormat % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
