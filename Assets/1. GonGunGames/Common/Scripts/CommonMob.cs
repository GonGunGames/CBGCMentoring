using System.Collections;
using UnityEngine;

public class CommonMob : BaseFSM
{
    public float idleTime = 0f;
    public float moveSpeed = 3f;
    public float turnSpeed = 180f;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float fastMoveSpeed = 4f;
    public float aggroTime = 3f;
    public ElliteHealth elliteHealth;
    private int attackCount = 0;
    private int maxAttacks = 3;
    private float attackCooldown = 0.7f;
    private bool isCooldown = false;
    private float sAttackDuration = 0.6f;
    public GameObject player; // 플레이어를 GameObject로 변경
    private FSMState previousState; // Hit 전 상태를 저장할 변수
    public CharacterController characterController; // 캐릭터 컨트롤러
    protected override void OnEnable()
    {
        base.OnEnable();
        elliteHealth = GetComponent<ElliteHealth>(); // EnemyHealth 컴포넌트를 가져옵니다.
        if (characterController != null)
        {
            characterController.enabled = true;
        }
        // player 태그를 가진 오브젝트를 찾아서 할당
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player with tag 'Player' not found in the scene.");
        }
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
            if (player != null && Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
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
            if (player != null)
            {
                MoveUtil.MoveFrame(controller, player.transform, moveSpeed, turnSpeed);

                if (Vector3.Distance(player.transform.position, transform.position) <= chaseRange)
                {
                    SetState(FSMState.Chase);
                }
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
            if (player != null && MoveUtil.MoveFrame(controller, player.transform, moveSpeed * 3.0f, turnSpeed) <= attackRange)
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
            if (player != null)
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
            }
            yield return null;
        }
    }

    protected override IEnumerator Attack()
    {
        while (!isNewState)
        {
            if (player != null)
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
            }
            yield return null;
        }
    }

    protected override IEnumerator SAttack()
    {
        // SAttack 상태 로직
        if (player != null)
        {
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
        
        if (elliteHealth.currentHealth <= 0)
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
        ReleaseToPool();
    }
    private void ReleaseToPool()
    {
        EnemyPoolManager poolManager = FindObjectOfType<EnemyPoolManager>();
        if (poolManager != null)
        {
            poolManager.ReleaseEnemy(gameObject);
        }
        else
        {
            Debug.LogError("EnemyPoolManager를 찾을 수 없습니다.");
        }
    }
    // 현재 상태를 저장하는 메서드
    public void SaveCurrentState(FSMState state)
    {
        previousState = state;
    }
    public void Initialize()
    {
        SetState(FSMState.Move);
        // 초기화 로직 추가
    }
}
