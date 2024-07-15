using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerFSMState
{
    Idle = 0,
    Run = 1,
    Attack = 2,
    Hit = 3,
    Dead = 4,
}

public class PlayerFSM : MonoBehaviour
{
    [SerializeField]
    PlayerFSMState Playerstate;

    protected bool isNewPlayerState = false;
    protected Animator animator;
    protected CapsuleCollider Playercollider;
    public PlayerFSMState CurrentState => Playerstate; // 현재 상태를 반환하는 프로퍼티 추가

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Playercollider = GetComponent<CapsuleCollider>();
        StartCoroutine(StateFSM());

        // 초기 상태를 Idle로 설정
        PlayerSetState(PlayerFSMState.Idle);
        Debug.Log("PlayerFSMStatePlay");
    }

    protected virtual void Update()
    {
    }

    IEnumerator StateFSM()
    {
        Debug.Log("StateFSMLog");
        while (true)
        {
            isNewPlayerState = false;
            Debug.Log("현재 상태: " + Playerstate.ToString()); // 현재 상태 출력
            yield return StartCoroutine(Playerstate.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
    }

    protected virtual IEnumerator Run()
    {
        yield return null;
    }

    protected virtual IEnumerator Hit()
    {
        yield return null;
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }
    protected virtual IEnumerator Dead()
    {
        yield return null;
    }

    public void PlayerSetState(PlayerFSMState newState)
    {
        isNewPlayerState = true;
        Playerstate = newState;
        animator.SetInteger("state", (int)Playerstate);

        Debug.Log(gameObject + ".SetState." + Playerstate.ToString());
    }
}