using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject rifle;
    public GameObject shotgun;
    public float weaponID;
    void Awake()
    {
        rifle = GameObject.FindWithTag("Rifle");
        shotgun = GameObject.FindWithTag("Shotgun");
        rifle.SetActive(false);
        shotgun.SetActive(false);
        weaponID = GetPlayerInfo.instance.GetStat(StatType.GunID);
        Debug.Log(weaponID);
        // 게임 시작 시 데이터 로드
        GetPlayerInfo.instance.LoadPlayerData();
    }

    private void Start()
    {
        // 데이터를 제대로 불러왔는지 확인
        if (GetPlayerInfo.instance.playerData == null)
        {
            Debug.LogError("Player data is not loaded. Please check the data loading process.");
            return;
        }

        
        Debug.Log("Player's Weapon ID: " + weaponID);

        // 무기 ID에 따라 무기 활성화
        if (111 <= weaponID && weaponID <= 113)
        {
            rifle.SetActive(true);
            Debug.Log("Rifle activated based on ID");
        }
        else if (114 <= weaponID && weaponID <= 116)
        {
            shotgun.SetActive(true);
            Debug.Log("Shotgun activated based on ID");
        }
        else
        {
            Debug.Log("No matching weapon found for ID: " + weaponID);
        }
    }
}
