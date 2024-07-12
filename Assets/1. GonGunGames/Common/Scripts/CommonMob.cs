using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMob : BaseFSM
{

    public float idleTime = 3f;
    public float moveSpeed = 3f;
    public float turnSpeed = 180f;
    public float chaseRange = 3f;
    public float attackRange = 1.5f;

    public float aggroTime = 3f;

    public List<Transform> moveMarkers;

    public GameObject player;

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator Idle()
    {
        float timer = 0f;

        while (!isNewState)
        {
            timer += Time.deltaTime;

            if (timer >= idleTime)
            {
                SetState(FSMState.Move);
            }

            if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
            {
                SetState(FSMState.Chase);
            }

            yield return null;
        }
    }
    protected override IEnumerator Move()
    {
        Transform moveMarker = moveMarkers[Random.Range(0, moveMarkers.Count)];

        while (!isNewState)
        {
            if (MoveUtil.MoveFrame(controller, moveMarker, moveSpeed, turnSpeed) <= 0.1f)
            {
                SetState(FSMState.Idle);
            }

            if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
            {
                SetState(FSMState.Chase);
            }

            yield return null;
        }
    }
    protected override IEnumerator Chase()
    {
        float timer = 0f;

        while (!isNewState)
        {
            timer += Time.deltaTime;

            if (timer >= aggroTime)
            {
                SetState(FSMState.Idle);
            }

            if (MoveUtil.MoveFrame(controller, player.transform, moveSpeed * 3.0f, turnSpeed) <= attackRange)
            {
                SetState(FSMState.Attack);
            }

            yield return null;
        }
    }
    protected override IEnumerator Hit()
    {
        while (!isNewState)
        {
            yield return null;
        }
    }
    IEnumerator Attack()
    {
        while (!isNewState)
        {

            if (animator.GetCurrentAnimatorStateInfo(0).length >= 0.9f)
            {
                SetState(FSMState.Chase);
            }

            yield return null;
        }
    }
}