using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    Transform initialParent;
    

    InventorySlot inventorySlot;
    EquipmentSlot equipmentSlot;

    EquipmentSlot equipableSlot;


    GameObject equipField;
    InventoryItem thisItem;

    private bool allowEquip = false;
    private bool allowUnequip = false;

    bool isEquip = false;

    public void EquipButtonClick(int isEquipState)
    {
        thisItem = GetComponent<InventoryItem>();

        initialParent = transform.parent;

        inventorySlot = transform.parent.GetComponent<InventorySlot>();


        if (inventorySlot != null)
        {
            inventorySlot.DisplayCountText(false, 0);
            inventorySlot.isEmpty = true;
            InventoryManager.Instance.scrollRect.vertical = false;

            if (!thisItem.data.info.prop.countable)
            {
                equipableSlot = InventoryManager.Instance.equipmentSlots
                    .Single(i => i.type == thisItem.data.info.baseStat.type);
            }
        }

        equipmentSlot = transform.parent.GetComponent<EquipmentSlot>();
        if (equipmentSlot != null)
        {
            equipmentSlot.isEquip = false;
        }
        Debug.Log("장비 전1");

        if (!thisItem.data.info.prop.countable)
        {
            //Debug.Log(equipableSlot);
            Debug.Log(transform.parent);
            //Debug.Log("장비 전2" + equipableSlot.isEquip);
            if (isEquipState == 0)
            {
                Debug.Log(equipableSlot + "111111111");
                equipmentSlot = transform.parent.GetComponent<EquipmentSlot>();
                Debug.Log(equipableSlot + "222222");
            }
            if (equipableSlot.isEquip && isEquipState == 0)
            {
                Debug.Log("해제");
                InventoryManager.Instance.UnequipItem(thisItem);
                equipableSlot.isEquip = false;

            }
            if (!equipableSlot.isEquip && isEquipState == 1)
            {
                Debug.Log("장착");
                InventoryManager.Instance.EquipItem(thisItem, equipableSlot);
                equipableSlot.isEquip = true;
            }
            if (isEquipState == 2)
            {
                Debug.Log("교체");
                InventoryItem equippedItem = equipableSlot.GetComponentInChildren<InventoryItem>();
                InventoryManager.Instance.ReplaceItem(thisItem, equippedItem);
            }
            Debug.Log("장비 후1");
        }
        Debug.Log("장비 후2" + equipableSlot.isEquip);

        equipField = GameObject.FindGameObjectWithTag("EquipField");

        InventoryItem targetItem = equipField.GetComponent<InventoryItem>();
        if (targetItem != null)
        {
            InventoryManager.Instance.ReplaceItem(thisItem, targetItem);
        }

        if (initialParent == transform.parent)
        {
            inventorySlot = GetComponentInParent<InventorySlot>();
            if (inventorySlot != null)
            {
                inventorySlot.isEmpty = false;
            }

            equipmentSlot = GetComponentInParent<EquipmentSlot>();
            if (equipmentSlot != null)
            {
                equipmentSlot.isEquip = true;
            }
        }

        Debug.Log(equipableSlot.type + "    " + equipableSlot.isEquip);
    }

    public void UnEquipButtonClick()
    {

    }
}
