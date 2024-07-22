using System.Collections;
using UnityEngine;

public class CommonMob : BaseFSM
{
    public float idleTime = 1.5f;
    public float moveSpeed = 3f;
    public float turnSpeed = 180f;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float fastMoveSpeed = 4f;
    public float aggroTime = 3f;
    public EnemyHealth health;
    private int attackCount = 0;
    private int maxAttacks = 3;
    private float attackCooldown = 0.7f;
    private bool isCooldown = false;
    private float sAttackDuration = 0.6f;

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
            if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
            {
                SetState(FSMState.Chase);
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

    protected override IEnumerator Hit()
    {
        // Hit 상태 로직
        // 피격 애니메이션 재생 등
        yield return new WaitForSeconds(0.5f); // Hit 애니메이션 시간만큼 대기

        if (health.currentHealth <= 0)
        {
            SetState(FSMState.Dead);
        }
        else
        {
            SetState(previousState); // 원래 상태로 복귀
        }
    }

    protected override IEnumerator Dead()
    {
        // Dead 상태에서 추가 로직 처리
        // 예: 애니메이션, 사운드 재생 등
        Debug.Log("Entering Dead State");

        // Dead 애니메이션 재생
        yield return new WaitForSeconds(1f); // Dead 애니메이션 시간만큼 대기

        // 프리팹 인스턴스화
        Instantiate(deathPrefab, transform.position, transform.rotation);

        // 애니메이션 재생 후 오브젝트 소멸
        Destroy(gameObject);
    }

    // 현재 상태를 저장하는 메서드
    public void SaveCurrentState(FSMState state)
    {
        previousState = state;
    }
}
