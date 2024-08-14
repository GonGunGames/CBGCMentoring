using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoCheck : MonoBehaviour
{
    public InventoryItem equipmentInfo;

    public GameObject weaponSlot;
    public GameObject armorSlot;
    public GameObject shoesSlot;
    public GameObject helmetSlot;
    public GameObject necklaceSlot;
    public GameObject ringSlot;

    public GameObject equipButton;

    Transform parentSetActive;

    private void Start()
    {
        parentSetActive = transform.parent.parent;
    }

    public void SlotCheck()
    {
        if (weaponSlot.transform.GetChild(5) != null)
        {
            equipButton.gameObject.SetActive(false);
        }
        else if (armorSlot.transform.GetChild(5) != null)
        {
            equipButton.gameObject.SetActive(false);
        }
        else if (shoesSlot.transform.GetChild(5) != null)
        {
            equipButton.gameObject.SetActive(false);
        }
        else if (helmetSlot.transform.GetChild(5) != null)
        {
            equipButton.gameObject.SetActive(false);
        }
        else if (necklaceSlot.transform.GetChild(5) != null)
        {
            equipButton.gameObject.SetActive(false);
        }
        else if (ringSlot.transform.GetChild(5) != null)
        {
            equipButton.gameObject.SetActive(false);
        }
        else
        {
            equipButton.gameObject.SetActive(true);
        }
    }

    public void OnClick(int state)
    {
        Debug.Log(equipmentInfo.data.info.name);
        Debug.Log(equipmentInfo.data.info.baseStat.type);

        EquipButton equipButton = equipmentInfo.GetComponent<EquipButton>();
        equipButton.EquipButtonClick(state);
        parentSetActive.gameObject.SetActive(false);
    }
}
