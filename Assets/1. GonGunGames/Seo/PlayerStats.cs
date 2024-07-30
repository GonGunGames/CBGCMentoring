using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemInfo;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats instance = null;
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

        if (DataPlayer.LoadData() != null)
        {
            playerData = DataPlayer.LoadData();
        }
    }

    public void GetStats()
    {

        for (int i = 0; i < playerData.baseStats.Length; i++)
        {
            playerStats[i] = playerData.additionalStats[i].value + playerData.baseStats[i].value;
            Debug.Log(playerStats[i]);
        }
    }

    void Start()
    {

        GetStats();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
