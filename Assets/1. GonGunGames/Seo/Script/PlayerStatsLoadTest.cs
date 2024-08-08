using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsLoadTest : MonoBehaviour
{
    public void OnClick()
    {
        //GetPlayerInfo.instance.GetStat(StatType.Health);
        Debug.Log(GetPlayerInfo.instance.GetStat(StatType.Health));
        Debug.Log(GetPlayerInfo.instance.GetPlayerEquipmentID(ItemType.Armor));
    }
}
