using UnityEngine;

public class GetPlayerInfo : MonoBehaviour
{
    public static GetPlayerInfo instance = null;
    public DataPlayer playerData;
    public DataInventory playerInventory;

    //public PlayerStat player;
    //public InventoryManager inventoryManager;

    float[] playerStats;

    int playerEquipmentsWeaponID;
    int playerEquipmentsArmorID;
    int playerEquipmentsGlovesID;
    int playerEquipmentsHelmetID;
    int playerEquipmentsPantsID;

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

        /*int Playerdamage;
        Playerdamage = GetPlayerInfo.instance.GetStat(StatType.Attack);

        weapomID = GetPlayerInfo.instance.GetPlayerEquipmentID(ItemType.Weapon);

        if(weapomID == 100)
        {

        }*/




    }

    public void SavePlayerData()
    {
        DataPlayer.SaveData(playerData);
        DataInventory.SaveData(playerInventory);
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
        // 모든 장비 스탯 가져오기 22ro
        LoadPlayerData();
        for (int i = 0; i < playerData.additionalStats.Length; i++)
        {
            playerStats[i] = playerData.additionalStats[i].value;

        }
        return playerStats;

    }

    public float[] GetAllBaseStats()
    {
        // 모든 플레이어 스탯 가져오기
        LoadPlayerData();
        for (int i = 0; i < playerData.baseStats.Length; i++)
        {
            playerStats[i] = playerData.baseStats[i].value;

        }
        return playerStats;

    }

    public float GetStat(StatType statType)
    {
        // 장비 + 플레이어 스탯 하나 가져오기
        LoadPlayerData();                   
        playerStats[((int)statType)] = playerData.additionalStats[((int)statType)].value + playerData.baseStats[((int)statType)].value;
        return playerStats[((int)statType)];

    }


    public float GetAdditionalStat(StatType statType)
    {
        // 장비 스탯 하나 가져오기
        LoadPlayerData();
        return playerData.baseStats[((int)statType)].value;
    }

    public float GetBaseStat(StatType statType)
    {
        // 플레이어 스탯 하나 가져오기
        LoadPlayerData();
        return playerData.additionalStats[((int)statType)].value;
    }

    public void SetPlayerEquipmentID(int ID, ItemType itemType)
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
        // 해당 장비의 ID 값 가져오기
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
