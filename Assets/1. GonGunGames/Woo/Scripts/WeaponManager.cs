using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject rifle_common;
    public GameObject rifle_uncommon;
    public GameObject rifle_rare;
    public GameObject shotgun_common;
    public GameObject shotgun_uncommon;
    public GameObject shotgun_rare;
    public GameObject sniper_common;
    public GameObject sniper_uncommon;
    public GameObject sniper_rare;

    public float weaponID;
    void Awake()
    {
        rifle_common.SetActive(false);
        rifle_uncommon.SetActive(false);
        rifle_rare.SetActive(false);
        shotgun_common.SetActive(false);
        shotgun_uncommon.SetActive(false);
        shotgun_rare.SetActive(false);
        sniper_common.SetActive(false);
        sniper_uncommon.SetActive(false);
        sniper_rare.SetActive(false);

        //rifleID = GetPlayerInfo.instance.GetStat(StatType.RifleID);
        //shotgunID = GetPlayerInfo.instance.GetStat(StatType.ShotgunID);
        //sniperID = GetPlayerInfo.instance.GetStat(StatType.SniperID);
        weaponID = GetPlayerInfo.instance.GetStat(StatType.GunID);
        // 게임 시작 시 데이터 로드
        GetPlayerInfo.instance.LoadPlayerData();
    }

    private void Start()
    {
        // 데이터를 제대로 불러왔는지 확인
        if (GetPlayerInfo.instance.playerData == null)
        {
            return;
        }
        if (weaponID == 111)
        {
            rifle_common.SetActive(true);
        }
        if (weaponID == 112)
        {
            rifle_uncommon.SetActive(true);
        }
        if (weaponID == 113)
        {
            rifle_rare.SetActive(true);
        }
        if (weaponID == 114)
        {
            shotgun_common.SetActive(true);
        }
        if (weaponID == 115)
        {
            shotgun_uncommon.SetActive(true);
        }
        if (weaponID == 116)
        {
            shotgun_rare.SetActive(true);
        }
        if (weaponID == 117)
        {
            sniper_common.SetActive(true);
        }
        if (weaponID == 118)
        {
            sniper_uncommon.SetActive(true);
        }
        if (weaponID == 119)
        {
            sniper_rare.SetActive(true);
        }
    }
}
