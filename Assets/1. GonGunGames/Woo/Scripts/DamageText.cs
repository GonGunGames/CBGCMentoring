using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public TextMeshPro damageText;  // TextMeshPro 3D 텍스트 컴포넌트
    public float displayDuration = 0.5f;  // 텍스트가 표시될 시간
    public float fadeSpeed = 0.2f; // 페이드 속도
    public float moveDistance = 0.2f; // 텍스트가 이동할 거리
    public float moveSpeed = 3.0f; // 이동 속도

    private void Start()
    {
        // 초기화 작업 없이 그대로 두기
    }

    public void SetDamage(float damage)
    {
        if (damageText != null)
        {
            damageText.text = damage.ToString();  // 데미지 값을 문자열로 변환하여 텍스트에 설정
            transform.rotation = Quaternion.Euler(new Vector3(70, 0, 0));

            // 초기 알파 값을 0으로 설정하여 페이드 인 시작
            Color color = damageText.color;
            color.a = 0;
            damageText.color = color;

            StartCoroutine(FadeInOutAndMove());    // 코루틴 시작
        }
        else
        {
            Debug.LogError("DamageText component is not assigned.");
        }
    }

    private IEnumerator FadeInOutAndMove()
    {
        Color color = damageText.color;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * moveDistance;

        // 서서히 나타나도록 페이드 인
        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeSpeed);
            damageText.color = color;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / fadeSpeed);
            yield return null; // 프레임 대기
        }
        color.a = 1; // 완전히 불투명하게 설정
        damageText.color = color;
        transform.position = targetPosition; // 최종 위치로 설정

        // 텍스트가 화면에 표시된 후 잠시 대기
        yield return new WaitForSeconds(displayDuration);

        // 서서히 사라지도록 페이드 아웃 및 다시 원위치로 이동
        elapsedTime = 0f;
        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeSpeed);
            damageText.color = color;
            transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / fadeSpeed);
            yield return null; // 프레임 대기
        }
        color.a = 0; // 완전히 투명하게 설정
        damageText.color = color;
        transform.position = startPosition; // 원위치로 설정

        Destroy(gameObject);  // 오브젝트 제거
    }
}