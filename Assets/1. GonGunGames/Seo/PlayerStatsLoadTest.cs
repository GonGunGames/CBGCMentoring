using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsLoadTest : MonoBehaviour
{
    public void OnClick()
    {
        //PlayerStats.instance.GetStats(StatType.Health);
        Debug.Log(PlayerStats.instance.GetStats(StatType.Health));
        
    }
}
