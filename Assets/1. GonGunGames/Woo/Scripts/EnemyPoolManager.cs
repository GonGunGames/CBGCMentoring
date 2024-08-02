using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPrefab
    {
        public int id;
        public GameObject prefab;
    }
    public Transform spawnCenter;  // 플레이어나 특정 오브젝트의 위치를 중심으로 설정
    public float minSpawnDistance = 20f;  // 스폰 최소 거리
    public float maxSpawnDistance = 30f;  // 스폰 최대 거리

    public List<EnemyPrefab> enemyPrefabs;  // 적 프리팹 리스트
    public int initialPoolSize = 10;  // 초기 풀 크기
    public float spawnInterval = 5f;  // 적 스폰 간격

    private Dictionary<int, ObjectPool<GameObject>> pools = new Dictionary<int, ObjectPool<GameObject>>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Awake()
    {
        // 각 적 프리팹에 대해 풀 생성
        foreach (var enemyPrefab in enemyPrefabs)
        {
            var pool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(enemyPrefab.prefab),
                actionOnGet: (obj) =>
                {
                    obj.SetActive(true);
                    InitializeEnemy(obj);  // 활성화 시 초기화
                },
                actionOnRelease: (obj) =>
                {
                    ReleaseEnemy(obj);  // 비활성화 시 상태 초기화
                    obj.SetActive(false);
                },
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: false,
                defaultCapacity: initialPoolSize,
                maxSize: 100
            );
            pools.Add(enemyPrefab.id, pool);
        }
    }

    public GameObject SpawnEnemy(int enemyId)
    {
        if (!pools.ContainsKey(enemyId))
        {
            Debug.LogError("Invalid enemy ID: " + enemyId);
            return null;
        }

        // Object Pool에서 적 가져오기
        var pool = pools[enemyId];
        GameObject enemy = pool.Get();
        activeEnemies.Add(enemy);

        // 적의 위치와 초기화 로직 설정 (예: 랜덤 위치)
        enemy.transform.position = GetRandomSpawnPosition();
        InitializeEnemy(enemy);  // 적의 위치를 설정한 후 초기화

        return enemy;
    }

    public void ReleaseEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);

            int enemyId = -1;

            // EnemyHealth 또는 ElliteHealth 컴포넌트 확인
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyId = enemyHealth.currentId;
            }
            else
            {
                var eliteHealth = enemy.GetComponent<ElliteHealth>();
                if (eliteHealth != null)
                {
                    enemyId = eliteHealth.currentId;
                }
            }

            if (enemyId != -1 && pools.ContainsKey(enemyId))
            {
                pools[enemyId].Release(enemy);
            }
            else
            {
                Destroy(enemy);
            }
        }
    }

    void InitializeEnemy(GameObject enemy)
    {
        // 적 초기화 로직 구현 (예: 체력 설정, AI 초기화 등)
        // 적 스크립트를 가져와서 초기화하는 예제
        var enemyScript = enemy.GetComponent<EnemyHealth>();
        if (enemyScript != null)
        {
            enemyScript.Initialize();
        }
        else
        {
            var eliteScript = enemy.GetComponent<ElliteHealth>();
            if (eliteScript != null)
            {
                eliteScript.Initialize();
            }
        }

        // AI 초기화 로직 추가
        var aiScript = enemy.GetComponent<CommonMobN>();
        if (aiScript != null)
        {
            aiScript.Initialize();  // AI를 초기화 또는 재시작
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // 중심점과의 거리 계산
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        float angle = Random.Range(0f, 2f * Mathf.PI);  // 0~2π 범위에서 각도 설정

        // 중심점을 기준으로 거리와 각도를 사용하여 스폰 위치 계산
        float x = spawnCenter.position.x + Mathf.Cos(angle) * distance;
        float z = spawnCenter.position.z + Mathf.Sin(angle) * distance;

        // y는 고정
        float y = spawnCenter.position.y;

        return new Vector3(x, y, z);
    }
}
