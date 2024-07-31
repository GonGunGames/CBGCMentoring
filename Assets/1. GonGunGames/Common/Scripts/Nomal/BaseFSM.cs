using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllUnits;
using UnityEngine.Rendering;

public enum FSMState
{
    Idle = 0,
    Move = 1,
    Chase = 2,
    Attack = 3,
    Hit = 4,
    Dead = 5,
    Fastmove = 6,
    SAttack = 7,
    RoarAttack = 8
}

//각 상태를 enum 변환
public class BaseFSM : MonoBehaviour
{
    [SerializeField]
    protected FSMState state = FSMState.Idle; // 접근자를 protected로 변경

    protected bool isNewState = false;

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected CharacterController controller;

    // Start is called before the first frame update

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }
    protected virtual void Start()
    {                
        StartCoroutine(FSM());
    }

    protected virtual void Update()
    {

    }

    IEnumerator FSM()
    {
        while (true)
        {
            isNewState = false;
            yield return StartCoroutine(state.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
    }
    protected virtual IEnumerator Move()
    {
        yield return null;
    }
    protected virtual IEnumerator Chase()
    {
        yield return null;
    }
    protected virtual IEnumerator Hit()
    {
        yield return null;
    }
    protected virtual IEnumerator Dead()
    {
        yield return null;
    }
    protected virtual IEnumerator Fastmove()
    {
        yield return null;
    }
    protected virtual IEnumerator Attack()
    {
        yield return null;
    }
    protected virtual IEnumerator SAttack()
    {
        yield return null;
    }
    protected virtual IEnumerator RoarAttack()
    {
        yield return null; // 기본적으로 아무 동작도 하지 않음
    }
    public void SetState(FSMState newState)
    {
        isNewState = true;
        state = newState;

        if (animator != null)
        {
            animator.SetInteger("state", (int)state);
        }
        else
        {
            Debug.LogWarning("Animator component is not assigned.");
        }

        Debug.Log(gameObject + ".SetState." + state.ToString());
    }
}
