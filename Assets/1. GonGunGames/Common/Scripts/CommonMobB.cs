using System.Collections;
using UnityEngine;

public class CommonMobB : BaseFSM
{
    public float idleTime = 1f;
    public float moveSpeed = 1f;
    public float turnSpeed = 180f;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float fastMoveSpeed = 2f;
    public float aggroTime = 3f;
    public BossHealth health;
    private int attackCount = 0;
    private int maxAttacks = 3;
    private float attackCooldown = 0.7f;
    private bool isCooldown = false;
    private float sAttackDuration = 0.6f;

    private float roarAttackInterval = 10f; // 10초 간격으로 RoarAttack 발동
    private float lastRoarAttackTime = 0f; // 마지막 RoarAttack 발동 시간
    public CharacterController characterController; // 캐릭터 컨트롤러
    public GameObject player;
    private FSMState previousState; // Hit 전 상태를 저장할 변수

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<BossHealth>(); // EnemyHealth 컴포넌트를 가져옵니다.
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        health = GetComponent<BossHealth>(); // EnemyHealth 컴포넌트를 가져옵니다.
        if (characterController != null)
        {
            characterController.enabled = true;
        }                              // player 태그를 가진 오브젝트를 찾아서 할당
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player with tag 'Player' not found in the scene.");
        }
        SetState(FSMState.Idle); // 초기 상태를 Idle로 설정
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
                yield break; // Idle 상태 종료
            }

            if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
            {
                SetState(FSMState.Chase);
                yield break; // Idle 상태 종료
            }

            // RoarAttack 상태로 전환 조건
            if (Time.time - lastRoarAttackTime >= roarAttackInterval)
            {
                SetState(FSMState.RoarAttack);
                yield break; // Idle 상태 종료
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
                yield break; // Idle 상태 종료
            }

            yield return null;
        }
    }

    protected override IEnumerator Move()
    {
        while (!isNewState)
        {
            MoveUtil.MoveFrame(controller, player.transform, moveSpeed, turnSpeed);

            if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
            {
                SetState(FSMState.Chase);
                yield break; // Move 상태 종료
            }

            // RoarAttack 상태로 전환 조건
            if (Time.time - lastRoarAttackTime >= roarAttackInterval)
            {
                SetState(FSMState.RoarAttack);
                yield break; // Move 상태 종료
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
                yield break; // Move 상태 종료
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
                yield break; // Chase 상태 종료
            }

            if (MoveUtil.MoveFrame(controller, player.transform, moveSpeed * 3.0f, turnSpeed) <= attackRange)
            {
                SetState(FSMState.Attack);
                yield break; // Chase 상태 종료
            }

            // RoarAttack 상태로 전환 조건
            if (Time.time - lastRoarAttackTime >= roarAttackInterval)
            {
                SetState(FSMState.RoarAttack);
                yield break; // Chase 상태 종료
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
                yield break; // Chase 상태 종료
            }

            yield return null;
        }
    }

    protected override IEnumerator Fastmove()
    {
        while (!isNewState)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            float currentSpeed = (distanceToPlayer <= chaseRange) ? fastMoveSpeed : moveSpeed;

            MoveUtil.MoveFrame(controller, player.transform, currentSpeed, turnSpeed);

            if (distanceToPlayer <= attackRange)
            {
                SetState(FSMState.Attack);
                yield break; // Fastmove 상태 종료
            }

            // RoarAttack 상태로 전환 조건
            if (Time.time - lastRoarAttackTime >= roarAttackInterval)
            {
                SetState(FSMState.RoarAttack);
                yield break; // Fastmove 상태 종료
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
                yield break; // Fastmove 상태 종료
            }

            yield return null;
        }
    }

    protected override IEnumerator Attack()
    {
        while (!isNewState)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > attackRange)
            {
                SetState(FSMState.Chase);
                yield break; // Attack 상태 종료
            }
            else
            {
                if (!isCooldown)
                {
                    attackCount++;
                    if (attackCount >= maxAttacks)
                    {
                        SetState(FSMState.SAttack);
                        yield break; // Attack 상태 종료
                    }
                    else
                    {
                        StartCoroutine(AttackCooldown());
                    }
                }
            }

            // RoarAttack 상태로 전환 조건
            if (Time.time - lastRoarAttackTime >= roarAttackInterval)
            {
                SetState(FSMState.RoarAttack);
                yield break; // Attack 상태 종료
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
                yield break; // Attack 상태 종료
            }

            yield return null;
        }
    }

    protected override IEnumerator SAttack()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer > attackRange)
        {
            SetState(FSMState.Chase);
            yield break; // SAttack 상태 종료
        }

        // SAttack 상태 로직 (한 번 실행)
        yield return new WaitForSeconds(sAttackDuration);

        attackCount = 0; // 공격 카운트 초기화
        SetState(FSMState.Attack);

        // 체력이 0이면 Dead 상태로 전환
        if (health.isDead)
        {
            SetState(FSMState.Dead);
        }
    }

    protected override IEnumerator RoarAttack()
    {
        // RoarAttack 전의 상태를 저장합니다
        FSMState previousState = state;

        // Roar 애니메이션 트리거
        animator.SetTrigger("Roar");

        // Roar 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(3f); // 애니메이션 시간에 맞게 조정

        // 마지막 RoarAttack 시간 업데이트
        lastRoarAttackTime = Time.time;

        // 저장된 상태로 돌아갑니다. 유효하지 않을 경우 기본 상태로 전환

        if (previousState == FSMState.Idle || previousState == FSMState.Move || previousState == FSMState.Chase || previousState == FSMState.Attack || previousState == FSMState.Fastmove || previousState == FSMState.SAttack)
        {
            Debug.Log("디버그로그 특수패턴");
            SetState(previousState);
        }
        else
        {
            SetState(FSMState.Idle); // 기본 상태로 전환
        }

        // 체력이 0인 경우 Dead 상태로 전환
        if (health.isDead)
        {
            SetState(FSMState.Dead);
        }
    }


    private IEnumerator AttackCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false;
    }

    protected override IEnumerator Hit()
    {
        previousState = state;

        animator.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);

        if (health.currentHealth <= 0)
        {
            SetState(FSMState.Dead);
        }
        else
        {
            if (previousState == FSMState.Idle || previousState == FSMState.Move || previousState == FSMState.Chase || previousState == FSMState.Attack || previousState == FSMState.Fastmove || previousState == FSMState.SAttack)
            {
                SetState(previousState);
            }
            else
            {
                SetState(FSMState.Idle);
            }
        }
    }

    protected override IEnumerator Dead()
    {
        Debug.Log("Entering Dead State");

        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
