using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector]
    public Transform initialParent;
    public Transform parentAfterDrag;

    public InventorySlot inventorySlot;
    public EquipmentSlot equipmentSlot;

    public EquipmentSlot equipableSlot;

    private InventoryItem thisItem;

    private bool allowDrag = true;
    private bool allowEquip = false;
    private bool allowUnequip = false;

    private void ResetParam()
    {
        allowEquip = false;
        allowUnequip = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PrepareForDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!allowDrag) return;

        HandleDragging(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleDrop(eventData);
    }

    private void PrepareForDrag()
    {
        thisItem = GetComponent<InventoryItem>();
        if (thisItem.gameObject.GetComponentInParent<Toggle>().isOn != true)
        {
            allowDrag = false;
            return;
        }
        

        initialParent = transform.parent;
        parentAfterDrag = transform.parent;

        inventorySlot = transform.parent.GetComponent<InventorySlot>();
        if (inventorySlot != null)
        {
            allowEquip = true;
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
            allowUnequip = true;
            equipmentSlot.isEquip = false;
        }

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    private void HandleDragging(PointerEventData eventData)
    {
        Vector3 mousePositionScreen = Input.mousePosition;
        mousePositionScreen.z = 0;
        mousePositionScreen.x -= Screen.width / 2;
        mousePositionScreen.y -= Screen.height / 2;

        transform.localPosition = mousePositionScreen;

        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        if (InventoryManager.Instance.ActiveSlot.GetComponentInChildren<InventorySlot>() != null)
        {
            if (equipableSlot == null) return;
            if (allowEquip && target.CompareTag("EquipField"))
            {
                InventoryManager.Instance.equipField.SetActive(true);
                equipableSlot.ShowCanEquip();
            }
            else
            {
                InventoryManager.Instance.equipField.SetActive(false);
                equipableSlot.ShowCannotEquip();
            }
        }
        else
        {
            if (allowUnequip && target.CompareTag("InventoryField"))
            {
                InventoryManager.Instance.inventoryField.SetActive(true);
            }
            else
            {
                InventoryManager.Instance.inventoryField.SetActive(false);
            }
        }
    }

    private void HandleDrop(PointerEventData eventData)
    {
        if (!allowDrag)
        {
            allowDrag = true;
            return;
        }

        thisItem.SetPosition(parentAfterDrag);

        InventoryManager.Instance.inventoryField.SetActive(false);
        InventoryManager.Instance.equipField.SetActive(false);
        InventoryManager.Instance.scrollRect.vertical = true;

        GameObject target = eventData.pointerEnter;

        Debug.Log(target.name);
        Debug.Log(target);

        if (!thisItem.data.info.prop.countable)
        {
            if (target.CompareTag("InventoryField"))
            {
                InventoryManager.Instance.UnequipItem(thisItem);
            }

            if (target.CompareTag("EquipField"))
            {
                if (!equipableSlot.isEquip)
                {
                    InventoryManager.Instance.EquipItem(thisItem, equipableSlot);
                }
                else
                {
                    InventoryItem equippedItem = equipableSlot.GetComponentInChildren<InventoryItem>();
                    InventoryManager.Instance.ReplaceItem(thisItem, equippedItem);
                }
            }
        }

        InventoryItem targetItem = target.GetComponent<InventoryItem>();
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

        ResetParam();
        image.raycastTarget = true;
    }
}
