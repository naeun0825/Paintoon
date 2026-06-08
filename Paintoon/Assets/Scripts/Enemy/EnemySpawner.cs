using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings (적 프리팹)")]
    public GameObject[] enemyPrefabs; // 여러 개의 적 프리팹을 담을 배열

    [Header("Spawn Settings (생성 설정)")]
    public float spawnInterval = 20f; // 20초마다 생성

    private float timer = 0f;
    private NavMeshTriangulation navMeshData; // 맵 전체 NavMesh 데이터를 저장할 변수

    void Start()
    {
        // 게임 시작, 맵 전체의 NavMesh 좌표 데이터를 가져와 저장
        navMeshData = NavMesh.CalculateTriangulation();
    }

    void Update()
    {
        if (GameStateManager.Instance == null || !GameStateManager.Instance.isGameStarted) return;

        // 매 프레임마다 시간을 누적
        timer += Time.deltaTime;

        // 누적된 시간이 spawnInterval(20초) 이상이 되면 적을 생성합니다.
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f; // 타이머 초기화
        }
    }

    void SpawnEnemy()
    {
        // 에러 방지: 프리팹 배열이 비어있거나 NavMesh 데이터가 없으면 실행하지 않음
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemySpawner에 할당된 적 프리팹이 없습니다!");
            return;
        }
        if (navMeshData.vertices.Length == 0)
        {
            Debug.LogWarning("구워진 NavMesh 데이터가 씬에 존재하지 않습니다!");
            return;
        }

        // 등록된 프리팹 중 하나를 무작위로 선택
        int randomPrefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomPrefabIndex];

        // 맵 전체 NavMesh의 꼭짓점 중 하나를 무작위로 선택
        int randomVertexIndex = Random.Range(0, navMeshData.vertices.Length);
        Vector3 spawnPosition = navMeshData.vertices[randomVertexIndex];

        // 찾은 안전한 위치에 선택된 프리팹을 생성
        Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"{selectedPrefab.name} 적이 맵 어딘가에 스폰되었습니다!");
    }
}