using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMob : BaseFSM
{

    public float idleTime = 1.5f;
    public float moveSpeed = 3f;
    public float turnSpeed = 180f;
    public float chaseRange = 3f;
    public float attackRange = 1.5f;
    public float fastMoveSpeed = 5f;
    public float aggroTime = 3f;

    private int attackCount = 0;
    private int maxAttacks = 3;
    private float attackCooldown = 0.7f;
    private bool isCooldown = false;
    private float sAttackDuration = 0.6f;

    public GameObject player;
    protected override void Start()
    {
        base.Start();
    }
    //override 부모클래스로부터 물려받은 함수를 자식 클래스에서 덮어 씌워서 자기만의 것으로 재정의 하는것
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
    //플레이
    protected override IEnumerator Move()
    {
        while (!isNewState)
        {
            MoveUtil.MoveFrame(controller, player.transform, moveSpeed, turnSpeed);
            if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
            {
                SetState(FSMState.Chase);
            }

            yield return null;
        }
    }
    //빨라짐
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
    protected override IEnumerator Fastmove()
    {
        while (!isNewState)
        {
            // 플레이어와의 거리가 추적 범위 이내인지 확인
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // 거리에 따라 이동 속도를 설정
            float currentSpeed = (distanceToPlayer <= chaseRange) ? fastMoveSpeed : moveSpeed;

            // 플레이어를 향해 이동
            MoveUtil.MoveFrame(controller, player.transform, currentSpeed, turnSpeed);

            // 공격 범위에 들어오면 공격 상태로 전환
            if (distanceToPlayer <= attackRange)
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
    protected override IEnumerator Attack()
    {
        while (!isNewState)
        {
            // 플레이어와 몬스터 사이의 거리를 계산
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // 만약 플레이어가 공격 범위를 벗어나면 Chase 상태로 전환
            if (distanceToPlayer > attackRange)
            {
                SetState(FSMState.Chase);
            }
            else
            {
                if (!isCooldown)
                {
                    attackCount++;
                    if (attackCount >= maxAttacks)
                    {
                        SetState(FSMState.SAttack);
                        yield break; // Attack 코루틴 종료
                    }
                    else
                    {
                        // 쿨타임 시작
                        StartCoroutine(AttackCooldown());
                    }
                }
            }
            yield return null;
        }
    }

    protected override IEnumerator SAttack()
    {
        // SAttack 상태 로직
        // 플레이어와 몬스터 사이의 거리를 계산
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // 만약 플레이어가 공격 범위를 벗어나면 Chase 상태로 전환
        if (distanceToPlayer > attackRange)
        {
            SetState(FSMState.Chase);
            yield break; // SAttack 코루틴 종료
        }

        // SAttack 상태 로직 (한 번 실행)
        // 여기서 SAttack 동작을 수행합니다.
        yield return new WaitForSeconds(sAttackDuration); // SAttack 애니메이션 시간만큼 대기

        // SAttack 완료 후 Attack 상태로 전환
        attackCount = 0; // 공격 카운트 초기화
        SetState(FSMState.Attack);
    }

    private IEnumerator AttackCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(attackCooldown); // Attack 애니메이션 시간만큼 대기
        isCooldown = false;
    }
    protected override IEnumerator Dead()
    {
        while (!isNewState)
        {
            yield return null;
        }
    }
}