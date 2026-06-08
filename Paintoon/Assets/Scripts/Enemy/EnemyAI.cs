using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // 상태 정의
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState;

    [Header("Target & Components")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator; // 애니메이션 제어용

    [Header("Detection Settings (시야 및 범위)")]
    public float sightRange = 10f;       // 인지 거리
    public float fieldOfViewAngle = 90f; // 시야각
    public float attackRange = 2f;       // 공격 거리
    public LayerMask obstacleMask;       // 벽 레이어 마스크

    [Header("Patrol Settings (배회 설정)")]
    public float patrolRadius = 10f;     // 배회 반경
    public float idleDuration = 1f;      // 목표 도착 후 대기 시간
    private bool isIdling = false;
    private float idleTimer = 0f;

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f;  // 공격 속도
    private float lastAttackTime;

    private NavMeshTriangulation navMeshData; // 맵 전체의 NavMesh 데이터를 담을 변수

    // 적의 이동 가능 여부를 제어하는 플래그
    public bool canMove = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = EnemyState.Patrol;

        navMeshData = NavMesh.CalculateTriangulation();
        
        StartCoroutine(SpawnSequence()); 
        agent.isStopped = true;
    }

    // 스폰 연출이 끝날 때까지 대기하는 코루틴
    IEnumerator SpawnSequence()
    {
        // 이동 금지 (Idle 상태 유지)
        canMove = false;

        // EnemyHealth 스크립트에 설정된 VFX 지속 시간을 가져옴
        float delay = 2.0f; 
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            delay = health.vfxDestroyTime;
        }

        // VFX가 터지는 시간만큼 대기
        yield return new WaitForSeconds(delay);

        yield return new WaitUntil(() => GameStateManager.Instance != null && GameStateManager.Instance.isGameStarted);
        // 대기 시간이 끝나면 이동 가능 상태로 전환하고 첫 목적지 설정
        canMove = true;
        SetNewPatrolDestination();
    }

    void Update()
    {
        // canMove가 false일 때(스폰 연출 중일 때)는 아래의 모든 이동/추적 로직을 무시함
        if (GameStateManager.Instance == null || !GameStateManager.Instance.isGameStarted) return;
        if (!canMove) return;
        
        
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                UpdatePatrolState(distanceToPlayer);
                break;
            case EnemyState.Chase:
                UpdateChaseState(distanceToPlayer);
                break;
            case EnemyState.Attack:
                UpdateAttackState(distanceToPlayer);
                break;
        }
    }

    // 상태별 로직
    void UpdatePatrolState(float distanceToPlayer)
    {
        // 시야각 내에 플레이어가 들어오면 추적 시작
        if (CanSeePlayer(distanceToPlayer))
        {
            isIdling = false; // 대기 상태 취소
            currentState = EnemyState.Chase;
            return;
        }

        // 목적지에 도착했는지 확인 (남은 거리가 0.5f 이하일 때)
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isIdling)
            {
                // 막 도착했다면 Idle 상태 진입
                isIdling = true;
                idleTimer = 0f;
                animator.SetBool("isMoving", false); // Idle 애니메이션 재생
            }
            else
            {
                // 대기 시간 카운트
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleDuration)
                {
                    // 1초가 지나면 새로운 목적지 탐색 및 이동 시작
                    isIdling = false;
                    SetNewPatrolDestination();
                    animator.SetBool("isMoving", true); // Walk 애니메이션 재생
                }
            }
        }
        else
        {
            // 이동 중일 때 애니메이션 유지
            animator.SetBool("isMoving", true);
        }
    }

    void UpdateChaseState(float distanceToPlayer)
    {

        agent.isStopped = false;
        agent.SetDestination(player.position);

        // 공격 범위에 들어오면 공격 시작
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        // 시야에서 벗어나면 다시 배회
        else if (!CanSeePlayer(distanceToPlayer))
        {
            currentState = EnemyState.Patrol;
            SetNewPatrolDestination(); // 배회로 돌아갈 때 즉시 새 목표 설정
        }
    }

    void UpdateAttackState(float distanceToPlayer)
    {
        // 이동 정지 및 플레이어 바라보기
        agent.isStopped = true;
        animator.SetBool("isMoving", false);

        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPos);

        // 쿨타임마다 계속 공격
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack"); // 공격 애니메이션 트리거
            PerformAttack();
            lastAttackTime = Time.time;
        }

        // 플레이어가 공격 범위 밖으로 도망가면 다시 추적
        if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    // 무작위 배회 위치 설정 
    void SetNewPatrolDestination()
    {
        // navMeshData.vertices 배열에서 무작위 꼭짓점 인덱스를 하나 뽑기
        int randomIndex = Random.Range(0, navMeshData.vertices.Length);

        // 해당 꼭짓점의 좌표를 다음 목표 지점으로 설정
        Vector3 randomDestination = navMeshData.vertices[randomIndex];

        agent.SetDestination(randomDestination);
        agent.isStopped = false;
    }

    // 시야 판정 (Raycast 추가)
    bool CanSeePlayer(float distance)
    {
        if (distance <= sightRange)
        {
            // 적의 정면과 플레이어 사이의 각도를 계산하여 시야각 내에 있는지 확인
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < fieldOfViewAngle / 2f)
            {
                // Raycast를 쏴서 플레이어와 적 사이에 벽이 없는지 확인
                if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer, distance, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void PerformAttack()
    {
        // PlayerHealth 싱글톤 인스턴스가 존재하면 1의 데미지를 가함
        if (PlayerHealth.Instance != null)
        {
            Debug.Log("적이 플레이어에게 데미지 1을 입힙니다!");
            PlayerHealth.Instance.TakeDamage(1);
        }
    }

    // Scene 화면 시각화
    private void OnDrawGizmos()
    {
        // 인지 범위 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 시야각 
        Vector3 rightDir = Quaternion.Euler(0, fieldOfViewAngle / 2f, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, -fieldOfViewAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, rightDir * sightRange);
        Gizmos.DrawRay(transform.position, leftDir * sightRange);
    }
}