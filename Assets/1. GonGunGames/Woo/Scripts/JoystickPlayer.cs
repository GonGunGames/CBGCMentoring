using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayer : MonoBehaviour
{
    public float speed = 10f; // 기본 속도 설정
    public FloatingJoystick floatingJoystick;
    public Rigidbody rb;
    public bool isRunning = false; // 달리는 상태를 나타내는 변수

    // Start 메소드에서 Rigidbody 초기화
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void FixedUpdate()
    {
        if (floatingJoystick != null && rb != null)
        {
            Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;

            // 이동 방향이 있을 때만 속도와 회전을 적용
            if (direction != Vector3.zero)
            {
                // 위치 설정
                Vector3 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);

                // 회전 설정
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.MoveRotation(toRotation);

                // 플레이어 상태를 달리는 상태로 설정
                isRunning = true;
            }
            else
            {
                // 이동 방향이 없으면 플레이어 상태를 달리는 상태가 아님(false)
                isRunning = false;
            }
        }
    }
}