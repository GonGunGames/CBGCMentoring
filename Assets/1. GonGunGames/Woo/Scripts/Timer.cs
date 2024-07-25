using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText; // 타이머를 표시할 Text UI
    private float startTime; // 게임 시작 시간
    private bool timerStarted = false; // 타이머가 시작되었는지 여부

    private void Start()
    {
        startTime = Time.time; // 게임 시작 시간 기록
        timerStarted = true;
    }

    private void Update()
    {
        if (!timerStarted)
        {
            return;
        }

        float elapsedTime = Time.time - startTime;
        DisplayTime(elapsedTime);
    }

    void DisplayTime(float timeToDisplay)
    {
        // 시간을 분과 초로 변환
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // 타이머 텍스트 업데이트
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        timerStarted = true;
    }

    public void StopTimer()
    {
        timerStarted = false;
    }

    public void UpdateDeathTime(float deathTime)
    {
        // 죽은 후 경과 시간을 타이머에 반영
        startTime = Time.time - deathTime;
        timerStarted = true;
    }
}