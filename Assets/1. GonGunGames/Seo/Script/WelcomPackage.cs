using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class WelcomPackage : MonoBehaviour
{
    public GameObject targetObject;
    public ItemBase.ItemData[] item;

    public DataPlayer dataPlayer;

    void Start()
    {
        if (dataPlayer.baseStats[23].value == 0)
        {
            targetObject.SetActive(true);
            InventoryManager.Instance.AddAmountOfItem(item[0], 1, 100);
            InventoryManager.Instance.AddAmountOfItem(item[1], 1, 111);
            InventoryManager.Instance.AddAmountOfItem(item[2], 1, 114);
            dataPlayer.baseStats[23].value++;
        }
        targetObject.SetActive(false);
    }

    public void Onclick()
    {
        targetObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
