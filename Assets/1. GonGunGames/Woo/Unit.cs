using System.Collections.Generic;
using UnityEngine;

namespace AllUnits
{
    [System.Serializable]
    public class Unit : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;
        public float damage;
        public bool isDamage;

        public Unit()
        {
            currentHealth = maxHealth;
        }
    }

    [System.Serializable]
    public class StatData
    {
        public List<Unit> units = new List<Unit>();
    }

    public class DataManager : MonoBehaviour
    {
        public void Init()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Data/Stats");
            StatData data = JsonUtility.FromJson<StatData>(textAsset.text);

            // 데이터 확인을 위해 로그 출력
            foreach (var unit in data.units)
            {
                Debug.Log($"Unit maxHealth: {unit.maxHealth}, damage: {unit.damage}");
            }
        }
    }
}