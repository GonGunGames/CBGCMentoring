using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // 따라다닐 타겟 오브젝트
    public Vector3 offset;          // 카메라와 타겟 간의 거리
    public float smoothSpeed = 0.125f;  // 카메라 이동 속도 (보간을 위한 값)
    public float shakeDuration = 0.2f;  // 흔들림 지속 시간
    public float shakeMagnitude = 0.3f; // 흔들림 세기

    private Vector3 originalPosition;
    private float currentShakeDuration = 0f;

    void LateUpdate()
    {
        if (currentShakeDuration > 0)
        {
            // 흔들림 효과 적용
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            // 흔들림 효과가 끝난 후 원래 위치로 되돌림
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void TriggerShake()
    {
        originalPosition = transform.position;
        currentShakeDuration = shakeDuration;
    }
}