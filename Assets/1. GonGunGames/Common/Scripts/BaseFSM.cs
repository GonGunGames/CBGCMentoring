using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FSMState
{
    Idle = 0,
    Move = 1,
    Chase = 2,
    Attack = 3,
    Hit = 4,
    Dead = 5,
    Fastmove = 6,
    SAttack = 7
}
//각상태를 enum 변환
public class BaseFSM : MonoBehaviour
{
    [SerializeField]
    FSMState state;

    protected bool isNewState = false;
    protected Animator animator;

    protected CharacterController controller;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(FSM());

        SetState(FSMState.Idle);
    }

    protected virtual void Update()
    {

    }


    IEnumerator FSM()
    {   
        while(true)
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
    protected void SetState(FSMState newState)
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
