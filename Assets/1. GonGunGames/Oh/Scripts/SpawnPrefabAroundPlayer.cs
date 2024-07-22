using UnityEngine;

public class SpawnPrefabAroundPlayer : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRadius = 5f;
    public int spawnCount = 3; // 소환할 개수
    public float spawnInterval = 10f; // 소환 간격

    void Start()
    {
        // 시작할 때 InvokeRepeating으로 일정 간격으로 소환을 시작합니다.
        InvokeRepeating("SpawnPrefabs", 0f, spawnInterval);
    }

    void SpawnPrefabs()
    {
        // 플레이어를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 플레이어의 위치를 얻습니다.
            Vector3 playerPosition = player.transform.position;

            // 지정된 개수만큼 반복하여 프리팹을 소환합니다.
            for (int i = 0; i < spawnCount; i++)
            {
                // 랜덤한 위치 계산을 위한 랜덤 벡터 생성
                Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;

                // 랜덤 위치 계산
                Vector3 spawnPosition = new Vector3(playerPosition.x + randomCircle.x, playerPosition.y, playerPosition.z + randomCircle.y);

                // 프리팹 소환
                GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                // 생성된 프리팹에 대한 추가 설정이 필요하다면 여기서 처리합니다.
                // 예: spawnedPrefab.GetComponent<YourComponent>().CustomMethod();
            }
        }
    }
}
