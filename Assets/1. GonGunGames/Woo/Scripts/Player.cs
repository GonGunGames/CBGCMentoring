using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : PlayerFSM
{
    public float moveSpeed = 3f;
    public JoystickPlayer Joystick; // JoystickPlayer 클래스 참조
    public Rigidbody rb;
    public PlayerHealth health; // PlayerHealth 클래스 참조
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
            if (Joystick != null && Joystick.isRunning)
            {
                Debug.Log("Idle -> Run");
                PlayerSetState(PlayerFSMState.Run);
            }
            if (health != null && health.isDead)
            {
                Debug.Log("Dead");
                PlayerSetState(PlayerFSMState.Dead);
            }
            if (health != null && health.isHit)
            {
                Debug.Log("Hit");
                PlayerSetState(PlayerFSMState.Hit);
            }
            yield return null;
        }
    }

    protected override IEnumerator Run()
    {
        while (!isNewPlayerState)
        {
            if (Joystick != null && Joystick.isRunning)
            {
            }
            else
            {
                PlayerSetState(PlayerFSMState.Idle);
            }
            if (health != null && health.isDead)
            {
                Debug.Log("Dead");
                PlayerSetState(PlayerFSMState.Dead);
            }
            if (health != null && health.isHit)
            {
                Debug.Log("Hit");
                PlayerSetState(PlayerFSMState.Hit);
            }
            yield return null;
        }
    }

    protected override IEnumerator Hit()
    {
        while (!isNewPlayerState)
        {
            if (Joystick != null && Joystick.isRunning)
            {
                Debug.Log("Hit -> Run");
                PlayerSetState(PlayerFSMState.Run);
            }
            else
            {
                PlayerSetState(PlayerFSMState.Idle);    
            }
            yield return null;
        }
    }

    protected override IEnumerator Dead()
    {
        Debug.Log("Dead 상태 시작");
        if (Joystick != null )
        {
            Joystick.gameObject.SetActive(false); // JoystickPlayer 오브젝트 비활성화
        }
        while (!isNewPlayerState)
        {
            yield return null;
        }
        Debug.Log("Dead 상태 종료");
    }
}