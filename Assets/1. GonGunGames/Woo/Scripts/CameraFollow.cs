using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // 따라다닐 타겟 오브젝트
    public Vector3 offset;          // 카메라와 타겟 간의 거리
    public float smoothSpeed = 0.125f;  // 카메라 이동 속도 (보간을 위한 값)

    void LateUpdate()
    {
        // 타겟 위치에 오프셋을 더해 원하는 카메라 위치 계산
        Vector3 desiredPosition = target.position + offset;

        // 부드럽게 이동하기 위해 현재 위치에서 원하는 위치로 보간
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치를 보간된 위치로 설정
        transform.position = smoothedPosition;
    }
}