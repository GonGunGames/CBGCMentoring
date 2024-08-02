using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public Text noticeText;  // UI Text를 참조할 변수
    public float displayDuration = 5f;  // 텍스트가 표시될 시간 (초)
    public Color warningColor = Color.red;  // 경고 색상
    public float fadeDuration = 1f;  // 페이드 인/아웃 시간

    void Start()
    {
        if (noticeText != null)
        {
            noticeText.gameObject.SetActive(false);  // 시작할 때 텍스트 비활성화
            StartCoroutine(DisplayNotice("a", 3 * 60));  // 3분 후 "a" 표시
            StartCoroutine(DisplayNotice("a", 6 * 60));  // 6분 후 "a" 표시
            StartCoroutine(DisplayNotice("b", 5 * 60));  // 5분 후 "b" 표시
            StartCoroutine(DisplayNotice("b", 9 * 60));  // 9분 후 "b" 표시
            StartCoroutine(DisplayNotice("c", 10 * 60));  // 10분 후 "c" 표시
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
            noticeText.gameObject.SetActive(true);  // 텍스트 활성화

            // 페이드 인
            yield return StartCoroutine(FadeTextToFullAlpha());

            yield return new WaitForSeconds(displayDuration);  // 표시 시간 대기

            // 페이드 아웃
            yield return StartCoroutine(FadeTextToZeroAlpha());

            noticeText.gameObject.SetActive(false);  // 텍스트 비활성화
        }
    }

    private IEnumerator FadeTextToFullAlpha()
    {
        noticeText.color = new Color(noticeText.color.r, noticeText.color.g, noticeText.color.b, 0);
        while (noticeText.color.a < 1.0f)
        {
            noticeText.color = new Color(noticeText.color.r, noticeText.color.g, noticeText.color.b, noticeText.color.a + (Time.deltaTime / fadeDuration));
            yield return null;
        }
    }

    private IEnumerator FadeTextToZeroAlpha()
    {
        noticeText.color = new Color(noticeText.color.r, noticeText.color.g, noticeText.color.b, 1);
        while (noticeText.color.a > 0.0f)
        {
            noticeText.color = new Color(noticeText.color.r, noticeText.color.g, noticeText.color.b, noticeText.color.a - (Time.deltaTime / fadeDuration));
            yield return null;
        }
    }
}
