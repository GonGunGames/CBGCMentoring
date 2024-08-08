using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStat : Singleton<PlayerStat>
{
    public DataPlayer playerData;

    [SerializeField] private TextMeshProUGUI playerCP_TMP;
    private long playerCP;

    public GameObject statsUIGroup;
    public UIStat[] statsUI;

    [SerializeField] private CPCalculator cpCalculator;
    public void Awake()
    {
        if (DataPlayer.LoadData() != null)
        {
            playerData = DataPlayer.LoadData();
        }
        //StartCoroutine(SaveDataPeriodically(10.0f)); //The data will be saved automatically once after 10 sec.
        statsUI = statsUIGroup.GetComponentsInChildren<UIStat>();

        InitializePlayerStatFromData();

        playerCP = playerData.combatPower;
        playerCP_TMP.text = playerCP.ToString();

        string path = Application.persistentDataPath + "/player.json";
        //print(path);
    }
    void InitializePlayerStatFromData()
    {
        //---Update total stats--- (this one will overlap the previous step)
        for (int i = 0; i < playerData.baseStats.Length; i++)
        {
            StatType statType = playerData.baseStats[i].type;
            if (statType == StatType.Attack)
                statsUI[0].statText.text = (playerData.baseStats[i].value + playerData.additionalStats[i].value).ToString();
            else if (statType == StatType.AttackSpeed)
                statsUI[1].statText.text = (playerData.baseStats[i].value + playerData.additionalStats[i].value).ToString();
            else if (statType == StatType.AttackRange)
                statsUI[2].statText.text = (playerData.baseStats[i].value + playerData.additionalStats[i].value).ToString();
            else if (statType == StatType.Health)
                statsUI[3].statText.text = (playerData.baseStats[i].value + playerData.additionalStats[i].value).ToString();
            else if (statType == StatType.Defense)
                statsUI[4].statText.text = (playerData.baseStats[i].value + playerData.additionalStats[i].value).ToString();
            else if (statType == StatType.MoveSpeed)
                statsUI[5].statText.text = (playerData.baseStats[i].value + playerData.additionalStats[i].value).ToString();
        }
    }
    public void AddItemStat(InventoryItem item)
    {

        int statLen = item.data.currentStat.Length;
        for (int i = 0; i < statLen; i++)
        {
            StatType statType = item.data.currentStat[i].type;
            float itemStat = item.data.currentStat[i].value;
            if (statType == StatType.AttackRange
                || statType == StatType.AttackSpeed)
            {
                playerData.additionalStats[GetIndex(statType)].value = itemStat;
            }
            else
            {
                playerData.additionalStats[GetIndex(statType)].value += itemStat;
            }
        }
        InitializePlayerStatFromData();

        playerCP += cpCalculator.GetItemCP(item);
        playerCP_TMP.text = playerCP.ToString();
        playerData.combatPower = playerCP;
    }
    public void RemoveItemStat(InventoryItem item)
    {
        int statLen = item.data.currentStat.Length;
        for (int i = 0; i < statLen; i++)
        {
            StatType statType = item.data.currentStat[i].type;
            float itemStat = item.data.currentStat[i].value;
            if (statType == StatType.AttackRange
                || statType == StatType.AttackSpeed)
            {
                playerData.additionalStats[GetIndex(statType)].value = 0;
            }
            else
            {
                playerData.additionalStats[GetIndex(statType)].value -= itemStat;
            }
        }
        InitializePlayerStatFromData();

        playerCP -= cpCalculator.GetItemCP(item);
        playerCP_TMP.text = playerCP.ToString();
        playerData.combatPower = playerCP;
    }
    public int GetIndex(StatType type)
    {
        for (int i = 0; i < playerData.baseStats.Length; i++)
        {
            if (playerData.baseStats[i].type == type) return i;
        }
        return -1;
    }

    /*IEnumerator SaveDataPeriodically(float interval)
    {
        while (true)
        {
            DataPlayer.SaveData(player);
            yield return new WaitForSeconds(interval);
        }
    }
    */
    public void OnApplicationQuit()
    {
        // Save any unsaved data here
        DataPlayer.SaveData(playerData);
    }
}
