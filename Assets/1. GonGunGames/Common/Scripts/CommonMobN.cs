using System.Collections;
using UnityEngine;

public class CommonMobN : BaseFSM
{
    public float idleTime = 1f;
    public float moveSpeed = 1f;
    public float turnSpeed = 180f;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float aggroTime = 3f;
    public EnemyHealth health;
    private int attackCount = 0;
    private int maxAttacks = 3;
    private float attackCooldown = 0.7f;
    private bool isCooldown = false;

    public GameObject player;
    [SerializeField] private GameObject deathPrefab; // Dead 상태에서 스폰할 프리팹
    private FSMState previousState; // Hit 전 상태를 저장할 변수

    protected override void Start()
    {
        base.Start();
        health = GetComponent<EnemyHealth>(); // EnemyHealth 컴포넌트를 가져옵니다.
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
            Debug.Log(player);
            if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
            {
                SetState(FSMState.Chase);
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
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
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
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
            if (MoveUtil.MoveFrame(controller, player.transform, moveSpeed * 1.0f, turnSpeed) <= attackRange)
            {
                SetState(FSMState.Attack);
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
            }

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
                        attackCount = 0; // 공격 카운트 초기화
                    }
                    else
                    {
                        // 쿨타임 시작
                        StartCoroutine(AttackCooldown());
                    }
                }
            }

            // 체력이 0이면 Dead 상태로 전환
            if (health.isDead)
            {
                SetState(FSMState.Dead);
            }

            yield return null;
        }
    }

    private IEnumerator AttackCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(attackCooldown); // Attack 애니메이션 시간만큼 대기
        isCooldown = false;
    }

    protected override IEnumerator Hit()
    {
        // Hit 상태로 전환 전에 현재 상태 저장
        previousState = state;

        // Hit 상태 로직
        // 피격 애니메이션 재생 등
        animator.SetTrigger("Hit"); // Hit 애니메이션 트리거

        yield return new WaitForSeconds(0.5f); // Hit 애니메이션 시간만큼 대기

        if (health.currentHealth <= 0)
        {
            SetState(FSMState.Dead);
        }
        else
        {
            if (previousState == FSMState.Idle || previousState == FSMState.Move || previousState == FSMState.Chase || previousState == FSMState.Attack)
            {
                SetState(previousState); // 원래 상태로 복귀
            }
            else
            {
                SetState(FSMState.Idle); // 다른 경우에는 Idle 상태로 복귀
            }
        }
    }

    protected override IEnumerator Dead()
    {
        // Dead 상태에서 추가 로직 처리
        // 예: 애니메이션, 사운드 재생 등
        Debug.Log("Entering Dead State");

        // Dead 애니메이션 재생
        animator.SetTrigger("Dead"); // Dead 애니메이션 트리거
        yield return new WaitForSeconds(1f); // Dead 애니메이션 시간만큼 대기

        // 프리팹 인스턴스화
        Instantiate(deathPrefab, transform.position, transform.rotation);

        // 애니메이션 재생 후 오브젝트 소멸
        Destroy(gameObject);
    }
}
