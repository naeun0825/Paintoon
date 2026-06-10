using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI")]
    public UnityEngine.UI.Slider hpSlider;

    private Camera mainCamera;

    private Animator animator;
    private bool isDead = false;

    [Header("VFX Settings")]
    public GameObject spawnVFXPrefab; // 스폰 시 터질 파티클 프리팹
    public GameObject deathVFXPrefab; // 사망 시 터질 파티클 프리팹
    public float vfxDestroyTime = 1.5f; // 이펙트가 화면에서 사라질 시간 (초)
    private void Start()
    {
        currentHealth = maxHealth;
        hpSlider.value = (float)currentHealth / maxHealth;

        mainCamera = Camera.main;
        animator = GetComponent<Animator>();

        if (spawnVFXPrefab != null)
        {
            // 이펙트를 현재 적의 위치와 회전값에 맞춰 생성
            GameObject spawnVFX = Instantiate(spawnVFXPrefab, transform.position, spawnVFXPrefab.transform.rotation);
            // vfxDestroyTime 초 뒤에 씬에서 이펙트 삭제 (메모리 최적화)
            Destroy(spawnVFX, vfxDestroyTime);
        }
    }

    void Update()
    {
        // 체력바가 항상 카메라를 바라봄
        hpSlider.transform.LookAt(hpSlider.transform.position + mainCamera.transform.rotation * Vector3.forward,mainCamera.transform.rotation * Vector3.up);
    }

    // 팀원의 레이캐스트 스크립트에서 호출될 피격 함수
    public void TakeDamage(int damage)
    {
        Debug.Log($"TakeDamage 호출! isDead: {isDead}, currentHealth: {currentHealth}");

        if (isDead) return;

        currentHealth -= damage;
        hpSlider.value = (float)currentHealth / maxHealth;
        Debug.Log($"{gameObject.name}이(가) 공격받았습니다! 남은 체력: {currentHealth}");

        if (currentHealth > 0)
        {
            // 체력이 남아있다면 Hit 애니메이션 실행
            animator.SetTrigger("Hit");
        }
        else
        {
            // 체력이 0 이하라면 Die 애니메이션 실행 
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Debug.Log($"{gameObject.name} 사망!");

        if (deathVFXPrefab != null)
        {
            // 사망 이펙트를 적의 위치에 생성
            GameObject deathVFX = Instantiate(deathVFXPrefab, transform.position, deathVFXPrefab.transform.rotation);
            Destroy(deathVFX, vfxDestroyTime);
        }

        // 사망 후 적의 이동 및 콜라이더 비활성화 등의 처리
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        this.enabled = false; // AI 스크립트 정지

        // 일정 시간 후 캐릭터 삭제
        Destroy(gameObject, 3f);
    }
}