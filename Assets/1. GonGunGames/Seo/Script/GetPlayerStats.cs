using UnityEngine;

public class GetPlayerStats : MonoBehaviour
{
    public static GetPlayerStats instance = null;
    public DataPlayer playerData;
    float[] playerStats;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);

        }

    }

    public void LoadPlayerData()
    {
        if (DataPlayer.LoadData() != null)
        {
            playerData = DataPlayer.LoadData();

        }
        playerStats = new float[playerData.baseStats.Length];
    }

    public int GetStatsLength()
    {
        LoadPlayerData();
        return playerStats.Length;

    }

    public float[] GetAllStats()
    {
        LoadPlayerData();
        for (int i = 0; i < playerData.baseStats.Length; i++)
        {
            playerStats[i] = playerData.additionalStats[i].value + playerData.baseStats[i].value;

        }
        return playerStats;

    }

    public float[] GetAllAdditionalStats()
    {
        LoadPlayerData();
        for (int i = 0; i < playerData.additionalStats.Length; i++)
        {
            playerStats[i] = playerData.additionalStats[i].value;

        }
        return playerStats;

    }

    public float[] GetAllBaseStats()
    {
        LoadPlayerData();
        for (int i = 0; i < playerData.baseStats.Length; i++)
        {
            playerStats[i] = playerData.baseStats[i].value;

        }
        return playerStats;

    }

    public float GetStats(StatType statType)
    {
        playerStats[((int)statType)] = playerData.additionalStats[((int)statType)].value + playerData.baseStats[((int)statType)].value;
        return playerStats[((int)statType)];
    }

    void Start()
    {
        //GetStats();

    }

}
