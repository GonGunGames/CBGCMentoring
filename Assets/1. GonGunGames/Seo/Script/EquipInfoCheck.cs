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
    public GameObject changeButton;

    Transform parentSetActive;

    private void Start()
    {
        parentSetActive = transform.parent.parent;
    }

    public void SlotCheck(ItemType itemType)
    {
        if (itemType == ItemType.Weapon)
        {
            if (weaponSlot.transform.childCount == 5)
            {
                changeButton.gameObject.SetActive(false);
                return;
            }
            else if (weaponSlot.transform.GetChild(5) != null)
            {
                equipButton.gameObject.SetActive(false);
                return;
            }
            return;
        }

        if (itemType == ItemType.Armor)
        {
            if (armorSlot.transform.childCount == 5)
            {
                changeButton.gameObject.SetActive(false);
                return;
            }
            else if (armorSlot.transform.GetChild(5) != null)
            {
                equipButton.gameObject.SetActive(false);
                return;
            }
            return;
        }

        if (itemType == ItemType.Pants)
        {
            if (shoesSlot.transform.childCount == 5)
            {
                changeButton.gameObject.SetActive(false);
                return;
            }
            else if (shoesSlot.transform.GetChild(5) != null)
            {
                equipButton.gameObject.SetActive(false);
                return;
            }
            return;
        }

        if (itemType == ItemType.Helmet)
        {
            if (helmetSlot.transform.childCount == 5)
            {
                changeButton.gameObject.SetActive(false);
                return;
            }
            else if (helmetSlot.transform.GetChild(5) != null)
            {
                equipButton.gameObject.SetActive(false);
                return;
            }
            return;
        }

        if (itemType == ItemType.Pet)
        {
            if (necklaceSlot.transform.childCount == 5)
            {
                changeButton.gameObject.SetActive(false);
                return;
            }
            else if (necklaceSlot.transform.GetChild(5) != null)
            {
                equipButton.gameObject.SetActive(false);
                return;
            }
            return;
        }

        if (itemType == ItemType.Gloves)
        {
            if (ringSlot.transform.childCount == 5)
            {
                changeButton.gameObject.SetActive(false);
                return;
            }
            else if (ringSlot.transform.GetChild(5) != null)
            {
                equipButton.gameObject.SetActive(false);
                return;
            }
            return;
        }

        equipButton.gameObject.SetActive(true);
    }

    public void OnClick(int state)
    {
        //Debug.Log(equipmentInfo.data.info.name);
        //Debug.Log(equipmentInfo.data.info.baseStat.type);

        EquipButton equipButton = equipmentInfo.GetComponent<EquipButton>();
        equipButton.EquipButtonClick(state);
        parentSetActive.gameObject.SetActive(false);
    }
}
