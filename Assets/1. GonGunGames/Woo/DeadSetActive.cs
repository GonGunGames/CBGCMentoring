using UnityEngine.UI;
using UnityEngine;

public class DeadSetActive : MonoBehaviour
{
    public PlayerHealth health;
    public Button retrybt;
    public Button lobby;
    public RawImage total;
    public Text totalElapsedTimeText; // 시작부터 죽을 때까지의 경과 시간을 표시할 UI Text
    public Text elapsedSinceDeathText; // 죽을 때까지의 경과 시간을 표시할 UI Text

    private float startTime;
    private float deathTime;
    private bool isPlayerDead = false;

    private void Start()
    {
        // 초기에 필요한 UI 요소를 설정합니다.
        retrybt.gameObject.SetActive(false);
        lobby.gameObject.SetActive(false);
        total.gameObject.SetActive(false);
        totalElapsedTimeText.gameObject.SetActive(true);
        elapsedSinceDeathText.gameObject.SetActive(false); // 처음에는 죽을 때까지의 시간은 숨깁니다.

        // 게임 시작 시간을 기록합니다.
        startTime = Time.time;
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
            lobby.gameObject.SetActive(true);
            total.gameObject.SetActive(true);
            totalElapsedTimeText.gameObject.SetActive(false); // 시작부터 죽을 때까지의 시간은 숨깁니다.
            elapsedSinceDeathText.gameObject.SetActive(true); // 죽을 때까지의 시간을 표시합니다.

            // 죽을 때까지의 경과 시간을 계산하여 표시합니다.
            float elapsedTime = deathTime - startTime;
            elapsedSinceDeathText.text =  FormatTime(elapsedTime);
        }
        else if (!isPlayerDead)
        {
            // 플레이어가 살아 있는 동안의 전체 경과 시간을 계산하여 표시합니다.
            float totalElapsedTime = Time.time - startTime;
            totalElapsedTimeText.text = FormatTime(totalElapsedTime);
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