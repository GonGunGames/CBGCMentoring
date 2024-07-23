using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public float minSpawnDistance = 10f; // 플레이어와의 최소 거리
    public float maxSpawnDistance = 20f; // 플레이어와의 최대 거리

    public List<SpawnSchedule> spawnSchedules; // 테이블과 호출 시간을 저장하는 리스트

    private GameObject playerInstance; // 플레이어 인스턴스
    private float startTime; // 게임 시작 시간을 기록

    private void Start()
    {
        playerInstance = GameObject.FindWithTag("Player");

        if (playerInstance == null)
        {
            Debug.LogError("Player instance not found. Make sure the player prefab is correctly instantiated and tagged.");
            return;
        }

        startTime = Time.time;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float currentTime = Time.time - startTime;

            foreach (var schedule in spawnSchedules)
            {
                if (currentTime >= schedule.triggerTime)
                {
                    MonsterTableEntry table = DataBase.Instance.spawnTables[schedule.tableId - 1];
                    SpawnMonstersFromTable(table);
                    schedule.triggerTime += schedule.interval; // 다음 호출 시간을 갱신
                }
            }

            yield return new WaitForSeconds(1f); // 매 초마다 스폰 스케줄 확인
        }
    }

    private void SpawnMonstersFromTable(MonsterTableEntry table)
    {
        foreach (int monsterId in table.monsterIds)
        {
            SpawnMonster(monsterId);
        }
    }

    private void SpawnMonster(int monsterId)
    {
        if (monsterId < 1 || monsterId > DataBase.Instance.monsterPrefabs.Length)
        {
            Debug.LogError("Invalid monster ID: " + monsterId);
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(DataBase.Instance.monsterPrefabs[monsterId - 1], spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (playerInstance == null)
        {
            Debug.LogError("Player instance is null.");
            return Vector3.zero;
        }

        Vector3 randomDirection = Random.insideUnitCircle * maxSpawnDistance; // 원 안의 임의의 점
        Vector3 spawnPosition = new Vector3(randomDirection.x, 0, randomDirection.y) + playerInstance.transform.position;

        while (Vector3.Distance(playerInstance.transform.position, spawnPosition) < minSpawnDistance)
        {
            randomDirection = Random.insideUnitCircle * maxSpawnDistance;
            spawnPosition = new Vector3(randomDirection.x, 0, randomDirection.y) + playerInstance.transform.position;
        }

        return spawnPosition;
    }
}

[System.Serializable]
public class SpawnSchedule
{
    public int tableId; // 호출할 스폰 테이블의 ID
    public float triggerTime; // 스폰 테이블을 호출할 시간
    public float interval; // 다음 호출까지의 간격
}