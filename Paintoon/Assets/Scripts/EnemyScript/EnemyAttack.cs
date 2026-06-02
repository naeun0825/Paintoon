using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어의 HP_Subject를 찾아 데미지 부여
            var health = other.gameObject.GetComponent<HP_Subject>();
            health?.TakeDamage(damage);
        }
    }
}