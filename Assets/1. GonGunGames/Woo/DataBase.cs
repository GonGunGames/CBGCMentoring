using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    static DataBase instance;

    public static DataBase Instance
    {
        get { return instance; }
    }

    public PlayerData playerData;
    public EnemyData enemyData;
    public List<WeaponInfo> weaponType;

    public List<MonsterTableEntry> spawnTables; // 몬스터 테이블 리스트
    public GameObject[] monsterPrefabs;

    private void Awake()
    {
        instance = this;

        LoadPlayerDataFromJson();
        LoadEnemyDataFromJson();
        LoadWeaponDataFromJson();
        LoadSpawnTablesFromJson();

        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("Save Player Data")]
    void SavePlayerDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Save Enemy Data")]
    void SaveEnemyDataToJson()
    {
        string jsonData = JsonUtility.ToJson(enemyData, true);
        string path = Path.Combine(Application.persistentDataPath, "enemyData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Save Weapon Data")]
    void SaveWeaponDataToJson()
    {
        WeaponList weaponList = new WeaponList { weaponData = weaponType.ToArray() };
        string jsonData = JsonUtility.ToJson(weaponList, true);
        string path = Path.Combine(Application.persistentDataPath, "weaponData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Load Player Data")]
    void LoadPlayerDataFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            Debug.LogError("Player data file not found at " + path);
        }
    }

    [ContextMenu("Load Enemy Data")]
    void LoadEnemyDataFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "enemyData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            enemyData = JsonUtility.FromJson<EnemyData>(jsonData);
        }
        else
        {
            Debug.LogError("Enemy data file not found at " + path);
        }
    }

    [ContextMenu("Load Weapon Data")]
    void LoadWeaponDataFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "weaponData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            WeaponList weaponList = JsonUtility.FromJson<WeaponList>(jsonData);
            weaponType = new List<WeaponInfo>(weaponList.weaponData);
        }
        else
        {
            Debug.LogError("Weapon data file not found at " + path);
        }
    }

    [ContextMenu("Load Spawn Tables")]
    void LoadSpawnTablesFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "spawnData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            SpawnTableList spawnTableList = JsonUtility.FromJson<SpawnTableList>(jsonData);
            spawnTables = new List<MonsterTableEntry>(spawnTableList.spawnData);
        }
        else
        {
            Debug.LogError("Spawn data file not found at " + path);
        }
    }

    // 특정 ID에 해당하는 적 데이터를 반환하는 함수 추가
    public EnemyInfo GetEnemyInfoById(int id)
    {
        if (enemyData != null && enemyData.enemyData != null)
        {
            foreach (EnemyInfo enemy in enemyData.enemyData)
            {
                if (enemy.id == id)
                {
                    return enemy;
                }
            }
        }
        return null;
    }
    public WeaponInfo GetWeaponInfoByGunId(int gunId)
    {
        foreach (WeaponInfo weapon in weaponType)
        {
            if (weapon.gunId == gunId)
            {
                return weapon;
            }
        }
        Debug.LogWarning($"Weapon with gunId {gunId} not found.");
        return null;
    }
}

[System.Serializable]
public class PlayerData
{
    public string name;
    public float maxHealth;
    public int currentLevel;
    public int currentExp;
    public int expToLevelUp;
    public string[] items;
}

[System.Serializable]
public class WeaponInfo
{
    public int gunId;
    public string name;
    public float bulletSpeed;
    public float attackSpeed;
    public float attackDamage;
    public float attackChance;
    public int bulletsPerShot;
    public float burstInterval;
    public float nextFireTime;
}

[System.Serializable]
public class WeaponList
{
    public WeaponInfo[] weaponData;
}

[System.Serializable]
public class EnemyInfo
{
    public int id;
    public string name;
    public float maxHealth;
    public float damage;
}

[System.Serializable]
public class EnemyData
{
    public EnemyInfo[] enemyData;
}

[System.Serializable]
public class MonsterTableEntry
{
    public int tableId;
    public int[] monsterIds;
}

[System.Serializable]
public class SpawnTableList
{
    public MonsterTableEntry[] spawnData;
}