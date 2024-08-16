using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // 일시정지 메뉴 UI 패널
    public DataPlayer dataPlayer; // DataPlayer 참조
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false); // 시작할 때 일시정지 메뉴를 숨김
    }

    // 일시정지 버튼이 눌렸을 때 호출될 메서드
    public void OnPauseButtonClicked()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // 게임을 일시정지하는 메서드
    private void PauseGame()
    {
        pauseMenuUI.SetActive(true); // 일시정지 메뉴 활성화
        Time.timeScale = 0f; // 게임 시간 정지
        isPaused = true;
    }

    // 게임을 다시 시작하는 메서드
    private void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // 일시정지 메뉴 비활성화
        Time.timeScale = 1f; // 게임 시간 재개
        isPaused = false;
    }

    // 로비로 돌아가는 버튼이 눌렸을 때 호출될 메서드
    public void OnReturnToLobbyButtonClicked()
    {
        DataPlayer.SaveData(dataPlayer);
        Time.timeScale = 1f; // 게임 시간 정상화
        isPaused = false; // 일시정지 상태 해제
        SceneManager.LoadScene("Lobby"); // 로비 씬 로드 (씬 이름에 따라 변경)
    }
}
