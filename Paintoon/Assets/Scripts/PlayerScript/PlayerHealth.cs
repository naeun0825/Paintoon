using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PlayerHealth Instance { get; private set; }

    [Header("Player Stats")]
    public int maxHealth = 5;
    private int currentHealth;

    // 옵저버 패턴: 체력이 변할 때마다 알림을 보낼 이벤트
    public event Action<int> OnHealthChanged;
    public event Action OnPlayerDeath;

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // 적이 공격할 때 호출할 함수
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // 이미 죽었다면 무시

        currentHealth -= damage;
        Debug.Log($"플레이어가 공격받았습니다! 남은 체력: {currentHealth}");

        // 체력이 변했다고 옵저버에게 알림
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
        OnPlayerDeath?.Invoke(); // 플레이어 사망 시 필요한 처리 
    }
}