using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerFSM
{
    public float moveSpeed = 3f;
    public JoystickPlayer Joystick; // JoystickPlayer 클래스 참조
    public Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        if (Joystick == null)
        {
            Joystick = FindObjectOfType<JoystickPlayer>();
        }

        if (Joystick == null)
        {
            Debug.LogError("JoystickPlayer를 찾을 수 없습니다. Joystick 변수가 설정되지 않았습니다.");
        }
        Debug.Log("HI");

        // 초기 상태를 Idle로 설정
        PlayerSetState(PlayerFSMState.Idle);
    }

    protected override IEnumerator Idle()
    {
        while (!isNewPlayerState)
        {
            // 조이스틱이 달리는 상태를 확인
            if (Joystick != null && Joystick.isRunning) // JoystickPlayer 클래스의 isRunning 값을 사용
            {
                Debug.Log("Idle -> Run");
                PlayerSetState(PlayerFSMState.Run);
            }
            yield return null;
        }
    }

    protected override IEnumerator Run()
    {
        Debug.Log("Run 상태 시작"); // Run 상태 시작 메시지 출력
        while (!isNewPlayerState)
        {
            // 조이스틱이 달리는 상태를 확인
            if (Joystick != null && Joystick.isRunning) // JoystickPlayer 클래스의 isRunning 값을 사용
            {
                Debug.Log("플레이어가 달리고 있습니다");
                // Run 상태에서는 상태를 계속 Run으로 설정할 필요 없음
            }
            else
            {
                Debug.Log("Run -> Idle");
                PlayerSetState(PlayerFSMState.Idle);
            }

            yield return null;
        }
        Debug.Log("Run 상태 종료"); // Run 상태 종료 메시지 출력
    }

    protected override IEnumerator Hit()
    {
        while (!isNewPlayerState)
        {
            yield return null;
        }
    }

    protected override IEnumerator Dead()
    {
        while (!isNewPlayerState)
        {
            PlayerSetState(PlayerFSMState.Dead);
            yield return null;
        }
    }
}