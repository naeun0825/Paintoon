using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

// 체력을 관리해주는 Subject
public class HP_Subject : MonoBehaviour, Subject
{
    // 등록된 Observer들을 관리할 리스트
    private List<Observer> observers = new List<Observer>();

    public float maxHealth = 3f;
    [SerializeField]private float currentHealth;

    public bool isDead => currentHealth <= 0;

    // 객체의 이름을 로그에 표시하기 위해 변수 설정
    private string unitName;

    void OnEnable()
    {
        unitName = gameObject.name;
        currentHealth = maxHealth;
        NotifyObservers();
    }

    public void RegisterObserver(Observer _observer)
    {
        // Observer 등록
        this.observers.Add(_observer);
    }

    public void RemoveObserver(Observer _observer)
    {
        // Observer 해제
        this.observers.Remove(_observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.ObserverUpdate(currentHealth / maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;

        Debug.Log("데미지 입음! 옵저버들에게 알립니다.");

        NotifyObservers(); // 체력 변경 알림

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            // 플레이어 사망 시 로직
            GameManager.Instance.GameOver();
        }
        else
        {
            // 적 사망 시 로직
            Debug.Log($"{gameObject.name}가 소멸합니다");
            Destroy(gameObject, 0.1f);
            GameManager.Instance.killEnemy();
        }
    }
}