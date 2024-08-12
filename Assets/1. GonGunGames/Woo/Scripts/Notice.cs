using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public Text noticeText;  // UI Text를 참조할 변수
    public float displayDuration = 5f;  // 텍스트가 표시될 시간 (초)
    public Color warningColor = Color.red;  // 경고 색상
    public AudioSource DinoSaur;
    public AudioSource Warning;

    void Start()
    {
        if (noticeText != null)
        {
            noticeText.gameObject.SetActive(false);  // 시작할 때 텍스트 비활성화
            StartCoroutine(DisplayWarning("적들이 몰려옵니다!", 1 * 60));  // 3분 후 "a" 표시
            StartCoroutine(DisplayWarning("적들이 몰려옵니다!", 2 * 60));  // 6분 후 "a" 표시
            StartCoroutine(DisplayNotice("어디선가 울음소리가 들려옵니다!", 3 * 60));  // 5분 후 "b" 표시
            StartCoroutine(DisplayNotice("어디선가 울음소리가 들려옵니다!", 4 * 60));  // 9분 후 "b" 표시
            StartCoroutine(DisplayNotice("울음소리가 가까워졌습니다!", 5 * 60));  // 10분 후 "c" 표시
        }
        else
        {
            Debug.LogError("Notice Text is not assigned.");
        }
    }

    private IEnumerator DisplayNotice(string message, float delay)
    {
        yield return new WaitForSeconds(delay);  // 지연 시간 대기

        if (noticeText != null)
        {
            noticeText.text = message;  // 텍스트 설정
            noticeText.color = warningColor;  // 경고 색상 설정
            DinoSaur.Play();
            noticeText.gameObject.SetActive(true);  // 텍스트 활성화
            yield return StartCoroutine(BlinkNotice(5, 0.5f));  // 블링크 완료 대기
            noticeText.gameObject.SetActive(false);  // 텍스트 비활성화
        }
    }

    private IEnumerator DisplayWarning(string message, float delay)
    {
        yield return new WaitForSeconds(delay);  // 지연 시간 대기

        if (noticeText != null)
        {
            noticeText.text = message;  // 텍스트 설정
            noticeText.color = warningColor;  // 경고 색상 설정
            Warning.Play();
            noticeText.gameObject.SetActive(true);  // 텍스트 활성화
            yield return StartCoroutine(BlinkNotice(5, 0.5f));  // 블링크 완료 대기
            noticeText.gameObject.SetActive(false);  // 텍스트 비활성화
        }
    }

    // 빠르게 나타났다 사라지는 함수
    private IEnumerator BlinkNotice(int blinkCount, float blinkInterval)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            noticeText.gameObject.SetActive(true);
            yield return new WaitForSeconds(blinkInterval);
            noticeText.gameObject.SetActive(false);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}