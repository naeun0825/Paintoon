using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Wandering")]
    public float range = 10.0f;
    public float waitTime = 2.0f;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float timer;

    [Header("Preset Fields")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject splashFx;

    [Header("Detection Settings")]
    [SerializeField] private float chaseRange = 10.0f;
    [SerializeField][Range(0, 360)] private float viewAngle = 90.0f; // 시야각 (90도면 좌우 45도씩)
    [SerializeField] private LayerMask targetMask; // 플레이어 레이어 (6번 레이어 등)
    [SerializeField] private LayerMask obstacleMask; // 벽 등 장애물 레이어

    [Header("Hit Settings")]
    [SerializeField] private float stopDuration = 20.0f; // 공격 받은 후 멈추는 시간
    private Coroutine hitCoroutine; // 중복 실행 방지용
    [SerializeField] private GameObject hitVFXPrefab;
    public float damage = 1.0f;

    public Transform target;

    public enum State { None, Idle, Chase, Attack }

    [Header("Debug")]
    public State state = State.None;
    public State nextState = State.None;
    private bool attackDone;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        state = State.None;
        nextState = State.Idle;
        timer = waitTime;

        // 타겟이 할당되지 않았다면 Player 태그로 찾기 
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    private void Update()
    {
        // 1. 스테이트 전환 판단
        if (nextState == State.None)
        {
            switch (state)
            {
                case State.Idle:
                    // 시야각 내에 플레이어가 있는지 확인
                    if (CanSeePlayer())
                    {
                        nextState = State.Chase;
                    }
                    break;
                case State.Chase:
                    // 거리가 가까워지면 공격
                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        nextState = State.Attack;
                    }
                    // 시야에서 사라지거나 너무 멀어지면 포기 (추격 로직은 선택에 따라 유지 가능)
                    else if (!CanSeePlayer() && Vector3.Distance(transform.position, target.position) > chaseRange)
                    {
                        nextState = State.Idle;
                    }
                    break;
                case State.Attack:
                    if (attackDone)
                    {
                        nextState = State.Idle;
                        attackDone = false;
                    }
                    break;
            }
        }

        // 2. 스테이트 초기화
        if (nextState != State.None)
        {
            state = nextState;
            nextState = State.None;
            switch (state)
            {
                case State.Attack:
                    agent.ResetPath();
                    // 공격 시 플레이어를 바라보게 함
                    LookAtTarget();
                    Attack();
                    break;
            }
        }

        // 3. 업데이트 로직
        UpdateStateLogic();
    }

    private void UpdateStateLogic()
    {
        if (hitCoroutine != null) return;

        if (state == State.Idle)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime && (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance))
            {
                agent.SetDestination(GetRandomLocation(range));
                timer = 0;
            }
        }
        else if (state == State.Chase && target != null)
        {
            agent.SetDestination(target.position);
        }

        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;
        if (state == State.Attack || hitCoroutine != null) isMoving = false;
        animator.SetBool("isWalking", isMoving);
    }

    // 시야각 및 장애물 체크 핵심 로직
    private bool CanSeePlayer()
    {
        if (target == null) return false;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 1. 거리 확인
        if (distanceToTarget <= chaseRange)
        {
            // 2. 각도 확인
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2f)
            {
                // 3. 장애물 확인 (Raycast)
                if (!Physics.Raycast(transform.position + Vector3.up, dirToTarget, distanceToTarget, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void LookAtTarget()
    {
        if (target == null) return;
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    Vector3 GetRandomLocation(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        return NavMesh.SamplePosition(randomDirection, out hit, radius, 1) ? hit.position : transform.position;
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
    }
    public void WhenAnimationDone() => attackDone = true;

    private void OnDrawGizmosSelected()
    {
        // 시야각 시각화
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Vector3 lookAngleL = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up) * transform.forward;
        Vector3 lookAngleR = Quaternion.AngleAxis(viewAngle / 2, Vector3.up) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, lookAngleL * chaseRange);
        Gizmos.DrawRay(transform.position, lookAngleR * chaseRange);
    }

    // 총알 맞음
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            if (hitVFXPrefab != null)
            {

                Debug.Log($"{gameObject.name}가 공격 받았습니다.");
                Instantiate(hitVFXPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
                var health = GetComponent<HP_Subject>();
                health?.TakeDamage(damage);
            }

            if (hitCoroutine != null) StopCoroutine(hitCoroutine);
            hitCoroutine = StartCoroutine(HitReaction());
        }

    }

    public void InstantiateFx()
    {
        if (splashFx != null)
        {
            Instantiate(splashFx, transform.position, Quaternion.identity);
        }
    }

    IEnumerator HitReaction()
    {
        animator.SetTrigger("isHit");
        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        yield return new WaitForSeconds(stopDuration);

        if (agent != null) agent.isStopped = false;

        hitCoroutine = null;
    }

}