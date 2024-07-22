using AlmostEngine.Screenshot.Extra;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
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


    private void Awake()
    {
        instance = this;

        LoadPlayerDataFromJson();

        DontDestroyOnLoad(gameObject);
        
    }


    [ContextMenu("Save Player Data")]
    void SavePlayerDataToJson()
    {
        Debug.Log("SavePlayerDataToJson.Start");

        string jsonData = JsonUtility.ToJson(playerData, true);

        string path = Path.Combine(Application.persistentDataPath, "playerData.json");

        File.WriteAllText(path, jsonData);
        Debug.Log("SavePlayerDataToJson.End");
    }

    [ContextMenu("Save Enemy Data")]
    void SaveEnemyDataToJson()
    {
        Debug.Log("SaveEnemyDataToJson.Start");

        string jsonData = JsonUtility.ToJson(enemyData, true);

        string path = Path.Combine(Application.persistentDataPath, "enemyData.json");

        File.WriteAllText(path, jsonData);
        Debug.Log("SaveEnemyDataToJson.End");
    }

    [ContextMenu("Load Player Data")]
    void LoadPlayerDataFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");

        string jsonData = File.ReadAllText(path);

        playerData = JsonUtility.FromJson<PlayerData>(jsonData);

    }


    [ContextMenu("Load Enemy Data")]
    void LoadEnemyDataFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "enemyData.json");

        string jsonData = File.ReadAllText(path);

        enemyData = JsonUtility.FromJson<EnemyData>(jsonData);

    }

}

[System.Serializable]
public class PlayerData
{
    public string name;

    public float maxHealth;
    public float damage;
}


[System.Serializable]
public class EnemyData
{
    public string name;
    public int level;
    public float maxHealth;
    public float damage;
}

