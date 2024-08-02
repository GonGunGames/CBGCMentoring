using UnityEngine;

public class GetPlayerInfo : MonoBehaviour
{
    public static GetPlayerInfo instance = null;
    public DataPlayer playerData;
    public DataInventory playerInventory;

    float[] playerStats;

    int playerEquipmentsWeaponID;
    int playerEquipmentsArmorID;
    int playerEquipmentsGlovesID;
    int playerEquipmentsHelmetID;
    int playerEquipmentsPantsID;

    private void Awake()
    {
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

    public float GetStat(StatType statType)
    {
        // 장비 + 플레이어 스탯
        LoadPlayerData();
        playerStats[((int)statType)] = playerData.additionalStats[((int)statType)].value + playerData.baseStats[((int)statType)].value;
        return playerStats[((int)statType)];
    }
    public float GetAdditionalStat(StatType statType)
    {
        // 장비 스탯
        LoadPlayerData();
        return playerData.baseStats[((int)statType)].value;
    }

    public float GetBaseStat(StatType statType)
    {
        // 플레이어 스탯
        LoadPlayerData();
        return playerData.additionalStats[((int)statType)].value;
    }

    public void SetPlayerEquipment(int ID, ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                playerEquipmentsWeaponID = ID;
                break;
            case ItemType.Armor:
                playerEquipmentsArmorID = ID;
                break;
            case ItemType.Pants:
                playerEquipmentsPantsID = ID;
                break;
            case ItemType.Helmet:
                playerEquipmentsHelmetID = ID;
                break;
            case ItemType.Gloves:
                playerEquipmentsGlovesID = ID;
                break;
            default:
                break;
        }
    }

    public int GetPlayerEquipmentID(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                return playerEquipmentsWeaponID;
            case ItemType.Armor:
                return playerEquipmentsArmorID;
            case ItemType.Pants:
                return playerEquipmentsPantsID;
            case ItemType.Helmet:
                return playerEquipmentsHelmetID;
            case ItemType.Gloves:
                return playerEquipmentsGlovesID;
            default:
                return 0;
        }
    }

    void Start()
    {
        //GetStats();

    }

}
