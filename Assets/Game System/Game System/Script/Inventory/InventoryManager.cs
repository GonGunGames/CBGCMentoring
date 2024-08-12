using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("General")]
    public Camera mainCamera;
    //public ItemInfo itemInfo;

    [Header("DataInventory")]
    public DataInventory inventoryData;
    public DataPlayer playerData;

    [Header("Inventory")]
    public Transform Tabs;
    public GridLayoutGroup Grid;
    public ScrollRect scrollRect;
    public Transform customCursor;
    public GameObject SlotLocked;
    public GameObject SlotFree;
    public GameObject ItemPrefab;
    public GameObject unlockNoti;
    public int inventorySize;
    public int inventoryLockedSize;
    public bool canExpand = false;
    public bool AddEmptyCells = true;


    [Header("Slots")]
    public List<GameObject> unlockedSlots = new List<GameObject>();
    public List<GameObject> lockSlots = new List<GameObject>();
    public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    public Toggle ActiveSlot;


    [Header("Item View")]
    public GameObject itemViewPanel;
    public TextMeshProUGUI itemNameUI;
    public Image itemBackgroundUI;
    public Image itemImageUI;
    public TextMeshProUGUI itemLevel;
    public TextMeshProUGUI itemRarity;
    public TextMeshProUGUI itemSpecialStat;
    public TextMeshProUGUI itemDescription;

    public GameObject verticalSlash;
    public GameObject itemViewStatGroup;
    public UIStat[] itemStats;


    public GameObject inventoryField;
    public GameObject equipField;

    public PlayerStat playerStat;

    public Sprite[] statSprites;

    public Button equipButton;
    public Button unequipButton;
    public EquipmentSlot equipableSlot;

    [Header("InventoryTab")]
    public ToggleGroup inventoryTabPool;
    public Toggle[] inventoryTabs;

    private int[] tabParameters;

    private int currentItemType;
    private int currentRarity;

    private void Awake()
    {

        DataInventory.LoadData(inventoryData);
        //Debug.Log(inventoryData);

        //Hide all the item stats in baseStat view
        itemStats = itemViewStatGroup.GetComponentsInChildren<UIStat>();
        foreach (UIStat stat in itemStats)
        {
            stat.gameObject.SetActive(false);
        }
        SpawnSlots();
        SpawnItemsFromData();

        currentItemType = 0;
        currentRarity = 0;
        //Allocate FilterItemType() function to Toggle and filter first tab
        inventoryTabs = inventoryTabPool.GetComponentsInChildren<Toggle>();
        tabParameters = new int[] { 0, 7, 8, 9, 11 };
        for (int i = 0; i < inventoryTabs.Length; i++)
        {
            Toggle tab = inventoryTabs[i];
            int tabPara = tabParameters[i];
            tab.group = inventoryTabPool;
            tab.onValueChanged.AddListener(delegate (bool isOn)
            {
                if (isOn)
                {
                    FilterItemType(tabPara);
                }
            });

        }
        FilterTypeAndRarity(currentItemType, currentRarity);


    }

    private void Update()
    {
        //Just check that InventorySystem is active or not => I just want to ultilize the customCursor instead of creating new parameter "GameObject inventorySystem"
        if (!customCursor.gameObject.activeInHierarchy) return;

        //Overall:
        //- If there is any slot is clicked => Display the item baseStat
        //- If users press left button to the active slot => They will take the item
        if (Grid.GetComponent<ToggleGroup>().AnyTogglesOn() == true)
        {
            ActiveSlot = Grid.GetComponent<ToggleGroup>().ActiveToggles().Single(i => i.isOn);
            DisplayItemInformation();
        }
        //----Display Item Information
        DisplayItemViewPanel();
    }
    private void SpawnSlots()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject addSlot = Instantiate(SlotFree, Grid.transform);
            addSlot.transform.localScale = Vector3.one;
            unlockedSlots.Add(addSlot);
            addSlot.GetComponent<Toggle>().group = GetComponentInParent<ToggleGroup>();
            if (i == 0)
            {
                addSlot.GetComponent<Toggle>().isOn = true;
            }
        }
        for (int i = 0; i < inventoryLockedSize; i++)
        {
            GameObject addSlot = Instantiate(SlotLocked, Grid.transform);
            lockSlots.Add(addSlot);
            addSlot.GetComponent<Toggle>().group = GetComponentInParent<ToggleGroup>();
        }
    }

    /// <summary>
    /// Spawn equipped and inventory items from data when the game started.
    /// </summary>
    private void SpawnItemsFromData()
    {
        FilterItemType(0);
        InitializeEquippedItemFromData();
    }



    /// <summary>
    /// Display item information in the display field.
    /// </summary>
    public void DisplayItemViewPanel()
    {
        if (ActiveSlot.GetComponentInChildren<InventoryItem>() == null) //Item Information disappear
        {
            itemViewPanel.SetActive(false);
        }
        else
        {
            itemViewPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Display item information in the item view panel.
    /// </summary>
    private void DisplayItemInformation()
    {
        InventoryItem currentItem = ActiveSlot.GetComponentInChildren<InventoryItem>();
        if (currentItem == null) return;
        //---Check special condition
        //----1. Check specialStat
        if (currentItem.data.info.prop.specialStat == "")
        {
            itemSpecialStat.gameObject.SetActive(false);
        }
        else
        {
            itemSpecialStat.gameObject.SetActive(true);
        }
        //----2. Check type of item => if (item == Potion, Book, Scroll, Material => only show Image, no level, no rarity)
        // or you can set your own type
        if (currentItem.data.info.baseStat.type == ItemType.Potion
            || currentItem.data.info.baseStat.type == ItemType.Book
            || currentItem.data.info.baseStat.type == ItemType.Scroll
            || currentItem.data.info.baseStat.type == ItemType.Material)
        {
            verticalSlash.SetActive(false);
            itemViewStatGroup.SetActive(false);
        }
        else if (currentItem.data.info.baseStat.type != ItemType.Currency) //----3. If Item is Weapon -> Gloves
        {
            verticalSlash.SetActive(true);
            itemViewStatGroup.SetActive(true);
            foreach (UIStat stat in itemStats)
            {
                stat.gameObject.SetActive(false);
            }
            //int statLen = currentItem.data.currentStat.Length;
            int statLen = currentItem.data.info.baseStat.stats.Length;
            if (statLen <= 3)
            {
                for (int i = 0; i < statLen; i++)
                {
                    itemStats[i].gameObject.SetActive(true);
                    itemStats[i].statImage.sprite = CheckStatImage(currentItem.data.info.baseStat.stats[i].type);
                    itemStats[i].statText.text = currentItem.data.info.baseStat.stats[i].value.ToString();
                }
            }
            else
            {
                DisplayWeaponStat(currentItem);
            }

        }

        //---Then, set main features
        Rarity rarity = currentItem.data.info.baseStat.rarity;
        string itemName = currentItem.data.info.prop.itemName;
        itemNameUI.text = ColoringText(rarity, itemName);

        itemBackgroundUI.sprite = currentItem.backGround.sprite;
        itemImageUI.sprite = currentItem.img.sprite;
        if (itemViewStatGroup.activeInHierarchy)
        {
            //itemLevel.text = "Require Level " + currentItem.data.info.baseStat.requiredLevel.ToString();
            itemLevel.text = "Level " + currentItem.data.currentLevel.ToString();

            itemRarity.text = GetRarityText(rarity);
        }
        if (itemSpecialStat.gameObject.activeInHierarchy)
        {
            itemSpecialStat.text = currentItem.data.info.prop.specialStat;
        }
        itemDescription.text = currentItem.data.info.prop.itemDescription;


        equipButton.onClick.AddListener(() =>
        {
            if (!currentItem.data.info.prop.countable)
            {
                equipableSlot = equipmentSlots.Single(i => i.type == currentItem.data.info.baseStat.type);
                if (equipableSlot != null && !equipableSlot.isEquip)
                {
                    EquipItem(currentItem, equipableSlot);

                }/*
                else
                {
                    InventoryItem equippedItem = equipableSlot.GetComponentInChildren<InventoryItem>();
                    ReplaceItem(currentItem, equippedItem);
                }*/
            }


        });

        unequipButton.onClick.AddListener(() =>
        {
            if (!currentItem.data.info.prop.countable)
            {
                equipableSlot = equipmentSlots.Single(i => i.type == currentItem.data.info.baseStat.type);

                if (equipableSlot != null && equipableSlot.isEquip)
                {
                    UnequipItem(currentItem);
                }
                /*else
                {
                    InventoryItem equippedItem = equipableSlot.GetComponentInChildren<InventoryItem>();
                    ReplaceItem(currentItem, equippedItem);
                }*/
            }
        });
    }

    void DisplayWeaponStat(InventoryItem currentItem)
    {
        int statLen = currentItem.data.info.baseStat.stats.Length;
        for (int i = 0; i < statLen; i++)
        {
            if (CkeckWeaponStat(currentItem, i))
            {
                itemStats[i].gameObject.SetActive(true);
                itemStats[i].statImage.sprite = CheckStatImage(currentItem.data.info.baseStat.stats[i].type);
                itemStats[i].statText.text = currentItem.data.info.baseStat.stats[i].value.ToString();
            }

        }
    }

    bool CkeckWeaponStat(InventoryItem currentItem, int i)
    {
        switch (currentItem.data.info.baseStat.stats[i].type)
        {
            case StatType.Attack:
                return true;
            case StatType.AttackRange:
                return true;
            case StatType.AttackSpeed:
                return true;
            case StatType.BulletSpread:
                return true;
            case StatType.ExplosionRange:
                return true;
            case StatType.ReloadTime:
                return true;
            default:
                return false;
        }
    }

    //Sprite CheckStatImage(ItemInfo.ItemStat.Stat stat)
    Sprite CheckStatImage(StatType stat)
    {
        switch (stat)
        {
            case StatType.AdditionalAttacksProbability:
                return statSprites[0];
            case StatType.Attack:
                return statSprites[1];
            case StatType.AttackPowerMultiplier:
                return statSprites[2];
            case StatType.AttackPowerMultiplierProbability:
                return statSprites[3];
            case StatType.AttackRange:
                return statSprites[4];
            case StatType.AttackSpeed:
                return statSprites[5];
            case StatType.Branche:
                return statSprites[6];
            case StatType.BulletSize:
                return statSprites[7];
            case StatType.BulletSpeed:
                return statSprites[8];
            case StatType.BulletSpread:
                return statSprites[9];
            case StatType.CriticalProbability:
                return statSprites[10];
            case StatType.Defense:
                return statSprites[11];
            case StatType.ExplosionRange:
                return statSprites[12];
            case StatType.GrenadeProbability:
                return statSprites[13];
            case StatType.Health:
                return statSprites[14];
            case StatType.MagazineSize:
                return statSprites[15];
            case StatType.MaxAdditionalAttacks:
                return statSprites[16];
            case StatType.MoveSpeed:
                return statSprites[17];
            case StatType.NoReloadTimeProbability:
                return statSprites[18];
            case StatType.PenetratingPower:
                return statSprites[19];
            case StatType.PenetratingProbability:
                return statSprites[20];
            case StatType.ReloadTime:
                return statSprites[21];
            default:
                return null;
        }
    }
    string GetRarityText(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Legendary:
                return "<color=red>Legendary</color>";
            case Rarity.Mythical:
                return "<color=yellow>Mythical</color>";
            case Rarity.Epic:
                return "<color=purple>Epic</color>";
            case Rarity.Rare:
                return "<color=#04f4f4>Rare</color>";
            case Rarity.Uncommon:
                return "<color=green>Uncommon</color>";
            case Rarity.Common:
                return "<color=white>Common</color>";
        }
        return "";
    }

    string ColoringText(Rarity rarity, string text)
    {
        switch (rarity)
        {
            case Rarity.Legendary:
                return "<color=red>" + text + "</color>";
            case Rarity.Mythical:
                return "<color=yellow>" + text + "</color>";
            case Rarity.Epic:
                return "<color=purple>" + text + "</color>";
            case Rarity.Rare:
                return "<color=#04f4f4>" + text + "</color>";
            case Rarity.Uncommon:
                return "<color=green>" + text + "</color>";
            case Rarity.Common:
                return "<color=white>" + text + "</color>";
        }
        return "";
    }

    /// <summary>
    /// Equip the item to given slot
    /// </summary>
    /// <param name="thisItem"></param> current item.
    /// <param name="equipableSlot"></param> the slot that can be equipped.
    public void EquipItem(InventoryItem thisItem, EquipmentSlot equipableSlot)
    {
        Debug.Log("EquipItem");

        if (ActiveSlot.GetComponentInChildren<InventorySlot>() == null) return;

        thisItem.SetPosition(equipableSlot.transform);

        equipableSlot.ShowCannotEquip(); // Undisplay the red frame
        equipableSlot.isEquip = true;

        inventoryData.RemoveInventoryData(thisItem.data.ID);
        inventoryData.AddEquipmentData(thisItem.data);


        playerStat.AddItemStat(thisItem);

        if (thisItem.data.info.baseStat.type == ItemType.Weapon)
        {
            playerData.additionalStats[(int)StatType.GunID].value = thisItem.data.info.baseStat.IDIDID;
        }
        DataInventory.SaveData(inventoryData);
        DataPlayer.SaveData(playerData);
    }
    /// <summary>
    /// Unequip the item
    /// </summary>
    /// <param name="thisItem"></param> current item
    public void UnequipItem(InventoryItem thisItem)
    {
        Debug.Log("UnequipItem");
        if (ActiveSlot.GetComponentInChildren<EquipmentSlot>() == null) return;

        //-----Step 1: Put item into nearest Slot that is Empty
        foreach (GameObject unlockedSlot in unlockedSlots)
        {
            InventorySlot slot = unlockedSlot.GetComponent<InventorySlot>();
            if (slot.isEmpty)
            {
                thisItem.SetPosition(slot.transform);
                slot.isEmpty = false;
                ActiveSlot.GetComponent<EquipmentSlot>().isEquip = false;
                ActiveSlot.GetComponent<EquipmentSlot>().ShowCannotEquip();

                inventoryData.RemoveEquipmentData(thisItem.data.ID);
                inventoryData.AddInventoryData(thisItem.data);

                playerStat.RemoveItemStat(thisItem);

                break;
            }
        }
        playerData.additionalStats[(int)StatType.GunID].value = 0;
        DataInventory.SaveData(inventoryData);
        DataPlayer.SaveData(playerData);
    }

    /// <summary>
    /// Replace/swap 2 items
    /// </summary>
    /// <param name="thisItem"></param> Dragging Item
    /// <param name="targetItem"></param> Other Item
    public void ReplaceItem(InventoryItem thisItem, InventoryItem targetItem)
    {
        Debug.Log("ReplaceItem");
        EquipmentSlot equipmentSlot = targetItem.GetComponentInParent<EquipmentSlot>();
        if (equipmentSlot != null) // Swap with equipped item
        {
            Debug.Log(equipmentSlot);
            thisItem.SetPosition(equipmentSlot.transform);
            equipmentSlot.ShowCannotEquip();
            equipmentSlot.isEquip = true;

            targetItem.SetPosition(ActiveSlot.transform);
            ActiveSlot.GetComponent<InventorySlot>().isEmpty = false;

            Debug.Log(ActiveSlot.transform);
            Debug.Log(equipmentSlot.transform);

            //Update the database
            inventoryData.RemoveEquipmentData(targetItem.data.ID);
            inventoryData.AddInventoryData(targetItem.data);

            inventoryData.RemoveInventoryData(thisItem.data.ID);
            inventoryData.AddEquipmentData(thisItem.data);

            playerStat.RemoveItemStat(targetItem);
            playerStat.AddItemStat(thisItem);
            if (thisItem.data.info.baseStat.type == ItemType.Weapon)
            {
                playerData.additionalStats[(int)StatType.GunID].value = thisItem.data.info.baseStat.IDIDID;
            }
            DataInventory.SaveData(inventoryData);
            DataPlayer.SaveData(playerData);
            return;
        }

        InventorySlot inventorySlot = targetItem.GetComponentInParent<InventorySlot>();
        if (inventorySlot != null) // Swap with another item in inventory
        {
            targetItem.SetPosition(ActiveSlot.transform);
            thisItem.SetPosition(inventorySlot.transform);
            ActiveSlot.GetComponent<InventorySlot>().isEmpty = false;
        }

        

        DataInventory.SaveData(inventoryData);
        DataPlayer.SaveData(playerData);
    }

    public void AddAmountOfItem(ItemBase.ItemData data, int amount, int id)
    {
        //Debug.Log($"data: {data}, data.info: {data?.info}, data.info.baseStat: {data?.info?.baseStat}");

        bool newItem = false;
        if (data.info.prop.countable)
        {
            if (inventoryData.GetAmount(data.info) == 0)
            {
                newItem = true;
            }
            else
            {
                foreach (GameObject unlockedSlot in unlockedSlots)
                {
                    InventorySlot slot = unlockedSlot.GetComponent<InventorySlot>();
                    if (!slot.isEmpty)
                    {
                        InventoryItem item = unlockedSlot.GetComponentInChildren<InventoryItem>();
                        if (item.data.info == data.info)
                        {
                            item.data.amount += amount;
                            //Add data to database
                            inventoryData.AddInventoryData(item.data);

                            slot.countText.text = item.data.amount.ToString();
                            break;
                        }
                    }
                }
            }
        }
        if (!data.info.prop.countable || newItem == true)
        {
            foreach (GameObject unlockedSlot in unlockedSlots)
            {
                InventorySlot slot = unlockedSlot.GetComponent<InventorySlot>();
                if (slot.isEmpty)
                {
                    GameObject itemAdd = Instantiate(ItemPrefab, transform.position, Quaternion.identity);

                    itemAdd.transform.SetParent(unlockedSlot.transform);
                    itemAdd.transform.SetSiblingIndex(4);
                    itemAdd.transform.localPosition = new Vector3(0, 1, 0);
                    itemAdd.transform.localScale = new Vector3(1, 1, 1);
                    slot.isEmpty = false;

                    //Initialize data for new item => Fix in the future because it should be init in shop system/craft/gacha.
                    //Means it should be initialized when the item was created, not when add to inventory.
                    InventoryItem item = itemAdd.GetComponent<InventoryItem>();
                    //item.data.ID = GenerateID();
                    //Debug.Log($"item.data.ID 할당 전: data.info: {data.info}, data.info.baseStat: {data.info.baseStat}, data.info.baseStat.IDIDID: {data.info.baseStat.IDIDID}");
                    item.data.ID = id;
                    item.data.info = data.info;
                    item.data.amount += amount;
                    item.data.currentStat = data.info.baseStat.stats;

                    //Add data to database
                    inventoryData.AddInventoryData(item.data);

                    if (data.info.prop.countable)
                    {
                        slot.countText.text = item.data.amount.ToString();
                    }
                    break;
                }
            }
        }
        DataInventory.SaveData(inventoryData);
        DataPlayer.SaveData(playerData);
    }
    public void RemoveAmountOfItem(ItemInfo info, int amount)
    {
        foreach (GameObject unlockedSlot in unlockedSlots)
        {
            InventorySlot slot = unlockedSlot.GetComponent<InventorySlot>();
            InventoryItem item = unlockedSlot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                if (item.data.info.prop.itemName == info.prop.itemName)
                {
                    item.data.amount -= amount;

                    if (item.data.amount == 0)
                    {
                        inventoryData.RemoveInventoryData(item.data.ID);
                        slot.DestroyItem();
                    }
                    else if (item.data.amount > 0)
                    {
                        //Update to database
                        inventoryData.AddInventoryData(item.data);
                        slot.countText.text = item.data.amount.ToString();
                    }
                    break;
                }
            }
        }
        DataInventory.SaveData(inventoryData);
        DataPlayer.SaveData(playerData);
    }

    /// <summary>
    /// Permanently delete the item. Means it is no longer saved in data.
    /// </summary>
    public void DeleteItem()
    {
        InventorySlot activeSlot = ActiveSlot.GetComponent<InventorySlot>();
        if (!activeSlot.isEmpty)
        {
            inventoryData.RemoveInventoryData(activeSlot.GetComponentInChildren<InventoryItem>().data.ID);
            activeSlot.DestroyItem();
        }
        DataInventory.SaveData(inventoryData);
        DataPlayer.SaveData(playerData);
    }

    public int GenerateID()
    {
        int randNum;
        do
        {
            randNum = Random.Range(1000, 10000);
        } while (inventoryData.inventoryData.Exists(x => x.ID == randNum));
        /* Exists is similar to Single:
         * - Exists used for List
         * - Single used for array
         */
        return randNum;
    }


    /// <summary>
    /// Get the amount of the item.
    /// </summary>
    public int GetAmountOfItem(ItemInfo info)
    {
        foreach (GameObject unlockedSlot in unlockedSlots)
        {
            InventoryItem item = unlockedSlot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                if (item.data.info.prop.itemName == info.prop.itemName)
                {
                    return item.data.amount;
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// Filter with rarity.
    /// </summary>
    /// <param name="rarity"></param> the rarity that user wants to filter.
    public void FilterRarity(int rarity)
    {
        currentRarity = rarity;
        FilterTypeAndRarity(currentItemType, currentRarity);
    }

    /// <summary>
    /// Filter inventory with given itemType
    /// itemType == 0 => All
    /// itemType == 7 => Filter with type < 7, which means Weapon, Armor, ... Necklace.
    /// itemType == item.type => Filter with specific type. For example, itemType = 9, filter with 9, which is Potion.
    /// <param name="itemType"></param> the type that user wants to filter.
    /// </summary>
    public void FilterItemType(int itemType)
    {
        currentItemType = itemType;
        FilterTypeAndRarity(currentItemType, currentRarity);
    }

    /// <summary>
    /// Filter with type and rarity.
    /// </summary>
    /// <param name="itemType"></param> the type that user wants to filter.
    /// <param name="rarity"></param> the rarity that user wants to filter.
    public void FilterTypeAndRarity(int itemType, int rarity)
    {
        if (inventoryData.inventoryData.Count == 0)
        {
            return;
        }
        RemoveAllItem();
        foreach (ItemBase.ItemData data in inventoryData.inventoryData)
        {
            if (itemType == 0
                || itemType == 7 && (int)data.info.baseStat.type < itemType
                || (int)data.info.baseStat.type == itemType)
            {
                if (rarity == 0 ||
                (int)data.info.baseStat.rarity == rarity)
                {
                    InitializeInventoryItemFromData(data);
                }
            }
        }
    }
    public void InitializeInventoryItemFromData(ItemBase.ItemData data) //I ultilize this function for filter item type too
    {
        foreach (GameObject unlockedSlot in unlockedSlots)
        {
            InventorySlot slot = unlockedSlot.GetComponent<InventorySlot>();
            if (slot.isEmpty)
            {
                GameObject itemAdd = Instantiate(ItemPrefab, transform.position, Quaternion.identity);

                itemAdd.transform.SetParent(unlockedSlot.transform);
                itemAdd.transform.SetSiblingIndex(4); //If it is the last => count text will be faded
                itemAdd.transform.localPosition = new Vector3(0, 1, 0); // Not Vector3.zero because the slot asset is 32x33 pixel while the item asset is 32x32 pixel.
                itemAdd.transform.localScale = new Vector3(1, 1, 1);
                slot.isEmpty = false;

                InventoryItem item = itemAdd.GetComponent<InventoryItem>();
                item.data = data;

                //Debug.Log("data " + data);
                //Debug.Log("info " + data.info);
                //Debug.Log("prop " + data.info.prop);
                //Debug.Log("countable " + data.info.prop.countable);


                if (data.info.prop.countable)
                {
                    slot.countText.text = inventoryData.GetAmount(data.info).ToString();
                }
                break;
            }

        }
    }

    public void InitializeEquippedItemFromData()
    {
        if (inventoryData.equipmentData.Count == 0)
        {
            return;
        }

        foreach (ItemBase.ItemData data in inventoryData.equipmentData)
        {
            foreach (EquipmentSlot slot in equipmentSlots)
            {
                if (slot.type == data.info.baseStat.type)
                {
                    GameObject itemAdd = Instantiate(ItemPrefab, transform.position, Quaternion.identity);
                    itemAdd.transform.SetParent(slot.transform);
                    itemAdd.transform.localPosition = Vector3.zero;
                    itemAdd.transform.localScale = new Vector3(1, 1, 1);

                    InventoryItem item = itemAdd.GetComponent<InventoryItem>();
                    item.data = data;

                    slot.isEquip = true;
                }
            }
        }
    }

    public int IsEmptySlot()
    {
        int i = 0;
        foreach (GameObject unlockedSlot in unlockedSlots)
        {
            InventorySlot slot = unlockedSlot.GetComponent<InventorySlot>();
            if (slot.isEmpty)
            {
                i++;
            }
        }
        if (i > 0)
            return i;
        return 0;
    }

    /// <summary>
    /// Just remove all items from the inventory, but not remove it from data.
    /// </summary>
    public void RemoveAllItem()
    {
        foreach (GameObject unlockedSlot in unlockedSlots)
        {
            InventorySlot slot = unlockedSlot.GetComponent<InventorySlot>();
            if (!slot.isEmpty)
            {
                slot.DestroyItem();
            }
        }
    }


    /// <summary>
    /// Rearrange the inventory.
    /// </summary>
    public void RearrangeItem()
    {
        //Update in the future
    }


    /// <summary>
    /// Exit the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("종료1");
        DataInventory.SaveData(inventoryData);
        Application.Quit();
        Debug.Log("종료2");
    }


    public void OnApplicationQuit()
    {
        Debug.Log("저장중");
        DataInventory.SaveData(inventoryData);
        Debug.Log("저장완료");
    }
}
