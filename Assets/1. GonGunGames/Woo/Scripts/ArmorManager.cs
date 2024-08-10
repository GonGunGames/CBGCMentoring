using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{

    public float armorID;
    public float glovesID;
    public float helmetID;
    public float pantsID;
    public GameObject armor_common;
    public GameObject gloves_common;
    public GameObject helmet_common;
    public GameObject pants_common;
    public GameObject armor_uncommon;
    public GameObject gloves_uncommon;
    public GameObject helmet_uncommon;
    public GameObject pants_uncommon;
    public GameObject armor_rare;
    public GameObject gloves_rare;
    public GameObject helmet_rare;
    public GameObject pants_rare;
    void Awake()
    {

        armor_common.SetActive(false);
        gloves_common.SetActive(false);
        helmet_common.SetActive(false);
        pants_common.SetActive(false);
        armor_uncommon.SetActive(false);
        gloves_uncommon.SetActive(false);
        helmet_uncommon.SetActive (false);
        pants_uncommon.SetActive(false); 
        armor_rare.SetActive(false);
        gloves_rare.SetActive (false);
        helmet_rare.SetActive(false);
        pants_rare.SetActive(false);
        armorID = GetPlayerInfo.instance.GetStat(StatType.ArmorID);
        glovesID = GetPlayerInfo.instance.GetStat(StatType.GlovesID);
        helmetID = GetPlayerInfo.instance.GetStat(StatType.HelmetID);
        pantsID = GetPlayerInfo.instance.GetStat(StatType.PantsID);
        Debug.Log("armor ID :"  + armorID);
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
        Debug.Log("Player's Weapon ID: " + armorID);

        // 무기 ID에 따라 무기 활성화
        if (armorID == 100)
        {
            armor_common.SetActive(true);
        }
        if (armorID == 101)
        {
            armor_uncommon.SetActive(true);
        }
        if (armorID == 102)
        {
            armor_rare.SetActive(true);
        }
        if (glovesID == 103)
        {
            gloves_common.SetActive(true);
        }
        if (glovesID == 104)
        {
            gloves_uncommon.SetActive(true);
        }
        if (glovesID == 105)
        {
            gloves_rare.SetActive(true);
        }
        if (helmetID == 106) 
        {
            helmet_common.SetActive(true);
        }
        if (helmetID == 107)
        {
            helmet_uncommon.SetActive(true);
        }
        if (helmetID == 108)
        {
            helmet_rare.SetActive(true);
        }
        if (pantsID == 109) 
        {
            pants_uncommon.SetActive(true);
        }
        if (pantsID == 110)
        {
            pants_uncommon.SetActive(true);
        }
        if (pantsID == 111)
        {
            pants_rare.SetActive(true);
        }
    }
}
