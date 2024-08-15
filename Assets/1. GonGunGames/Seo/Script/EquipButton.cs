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

        if (!thisItem.data.info.prop.countable)
        {
            if (isEquipState == 0)
            {
                equipmentSlot = transform.parent.GetComponent<EquipmentSlot>();
            }
            if (equipableSlot.isEquip && isEquipState == 0)
            {
                InventoryManager.Instance.UnequipItem(thisItem);
                equipableSlot.isEquip = false;

            }
            if (!equipableSlot.isEquip && isEquipState == 1)
            {
                InventoryManager.Instance.EquipItem(thisItem, equipableSlot);
                equipableSlot.isEquip = true;
            }
            if (isEquipState == 2)
            {
                InventoryItem equippedItem = equipableSlot.GetComponentInChildren<InventoryItem>();
                InventoryManager.Instance.ReplaceItem(thisItem, equippedItem);
            }
        }

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

    }

    public void UnEquipButtonClick()
    {

    }
}
