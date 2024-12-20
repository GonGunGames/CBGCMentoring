using UnityEngine;
[CreateAssetMenu(fileName = "new Item", menuName = "Equipment")]
public class ItemInfo : ScriptableObject
{
    [System.Serializable]
    public class ItemProperties
    {
        public Sprite image;
        public Color color = Color.white;
        public bool countable;
        public string itemName;
        [TextArea(2, 2)]
        public string specialStat;
        [TextArea(3, 3)]
        public string itemDescription;
    }
    [System.Serializable]
    public class ItemStat
    {
        public int IDIDID;
        public ItemType type;
        public Rarity rarity;
        public int requiredLevel = 1;
        [SerializeField]
        public Stat[] stats;

        [System.Serializable]
        public class Stat
        {
            public StatType type;
            public float value;

            public float GetNextValue()
            {
                if (type == StatType.AttackRange || 
                    type == StatType.AttackSpeed || 
                    type == StatType.BulletSpeed ||
                    type == StatType.ReloadTime ||
                    type == StatType.MagazineSize ||
                    type == StatType.GunID ||
                    type == StatType.ArmorID ||
                    type == StatType.GlovesID ||
                    type == StatType.HelmetID ||
                    type == StatType.PantsID
                    ) 
                {
                    return value;
                }
                else
                {
                    return Mathf.CeilToInt(value * 1.05f);
                }
            }

        }
    }
    public ItemProperties prop;
    public ItemStat baseStat;

}