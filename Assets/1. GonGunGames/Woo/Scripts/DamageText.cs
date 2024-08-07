using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public TextMeshPro damageText;  // TextMeshPro 3D 텍스트 컴포넌트
    public float displayDuration = 0.2f;  // 텍스트가 표시될 시간
    public float fadeSpeed = 0.2f; // 페이드 속도
    public float moveDistance = 0.6f; // 텍스트가 이동할 거리
    public float moveSpeed = 3.0f; // 이동 속도
    public float popScaleFactor = 1.5f; // 텍스트 크기 팝 효과 배율
    public float popDuration = 0.2f; // 팝 효과 지속 시간
    public float bounceDistance = 0.5f; // 튀어오르는 최대 거리
    public float bounceDuration = 0.2f; // 튀어오르는 지속 시간

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

            // 팝 효과를 위한 초기 크기 설정
            Vector3 startScale = transform.localScale;
            Vector3 popScale = startScale * popScaleFactor;
            transform.localScale = popScale;

            // 튀어오르는 애니메이션을 위한 초기 설정
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + Random.onUnitSphere * moveDistance;

            StartCoroutine(FadeInOutAndMove(startPosition, targetPosition, startScale, popScale));    // 코루틴 시작
        }
        else
        {
            Debug.LogError("DamageText component is not assigned.");
        }
    }

    private IEnumerator FadeInOutAndMove(Vector3 startPosition, Vector3 targetPosition, Vector3 startScale, Vector3 popScale)
    {
        Color color = damageText.color;
        float elapsedTime = 0f;

        float popElapsedTime = 0f;

        // 서서히 나타나도록 페이드 인 및 팝 효과 동시에 진행
        while (elapsedTime < fadeSpeed || popElapsedTime < popDuration)
        {
            if (elapsedTime < fadeSpeed)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, elapsedTime / fadeSpeed);
                damageText.color = color;
            }

            if (popElapsedTime < popDuration)
            {
                popElapsedTime += Time.deltaTime;
                float scaleLerp = Mathf.Lerp(popScaleFactor, 1, popElapsedTime / popDuration);
                transform.localScale = startScale * scaleLerp;
            }

            // 위치 이동
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / fadeSpeed);

            yield return null; // 프레임 대기
        }

        // 페이드 인 완료 및 크기 최종 설정
        color.a = 1;
        damageText.color = color;
        transform.localScale = startScale; // 최종 크기로 설정
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

            // 텍스트 크기 줄이기
            float scaleLerp = Mathf.Lerp(1, popScaleFactor, elapsedTime / fadeSpeed);
            transform.localScale = startScale * scaleLerp;

            // 위치 이동
            transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / fadeSpeed);

            yield return null; // 프레임 대기
        }
        color.a = 0; // 완전히 투명하게 설정
        damageText.color = color;
        transform.position = startPosition; // 원위치로 설정

        Destroy(gameObject);  // 오브젝트 제거
    }
}
