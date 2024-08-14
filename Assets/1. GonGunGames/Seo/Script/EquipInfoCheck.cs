using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfoCheck : MonoBehaviour
{
    public InventoryItem equipmentInfo;

    Transform parentSetActive;

    private void Start()
    {
        parentSetActive = transform.parent.parent;
    }

    public void OnClick(int state)
    {
        Debug.Log(equipmentInfo.data.info.name);
        EquipButton equipButton = equipmentInfo.GetComponent<EquipButton>();
        equipButton.EquipButtonClick(state);
        parentSetActive.gameObject.SetActive(false);
    }
}
