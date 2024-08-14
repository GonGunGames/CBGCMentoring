using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    [HideInInspector]
    public Transform initialParent;
    

    public InventorySlot inventorySlot;
    public EquipmentSlot equipmentSlot;

    public EquipmentSlot equipableSlot;


    GameObject equipField;
    private InventoryItem thisItem;

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
            if (equipableSlot.isEquip && isEquipState == 0)
            {
                Debug.Log("해제");
                InventoryManager.Instance.UnequipItem(thisItem);
                equipableSlot.isEquip = false;

            }
            else if (!equipableSlot.isEquip && isEquipState == 1)
            {
                Debug.Log("장착");
                InventoryManager.Instance.EquipItem(thisItem, equipableSlot);
                equipableSlot.isEquip = true;
            }
            else if (isEquipState == 2)
            {
                Debug.Log("교체");
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

        Debug.Log(equipableSlot.type + "    " + equipableSlot.isEquip);
    }

    public void UnEquipButtonClick()
    {

    }
}
