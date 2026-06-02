using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 坷府 橇府普
    public float spawnInterval = 3.0f; // 利 积己 埃拜
    public float range = 20.0f; // 积己 馆版

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy(); // 利 积己
            timer = 0;
        }

        void SpawnEnemy()
        {
            Vector3 randomPos = Random.insideUnitSphere * range;
            randomPos.y = 0;
            Vector3 spawnPos = transform.position + randomPos;

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            GameManager.Instance.SpawnEnemy();
        }
    }
}
