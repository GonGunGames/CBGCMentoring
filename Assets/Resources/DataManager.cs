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
            currentHealth = maxHealth - damage;
        }
    }

    [System.Serializable]
    public class StatData
    {
        public List<Unit> units = new List<Unit>();
    }

    public class DataManager : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("DataManager Start called");
            Init();
        }

        public void Init()
        {
            Debug.Log("Init method called");

            // Step 1: Load the JSON file
            TextAsset textAsset = Resources.Load<TextAsset>("Data/Stats");
            if (textAsset == null)
            {
                Debug.LogError("Failed to load Stats data! The textAsset is null.");
                return;
            }

            Debug.Log("Stats data loaded successfully");

            // Step 2: Parse the JSON data
            StatData data = JsonUtility.FromJson<StatData>(textAsset.text);
            if (data == null || data.units == null)
            {
                Debug.LogError("Failed to parse Stats data! The parsed data is null.");
                return;
            }

            Debug.Log("Stats data parsed successfully");

            // Step 3: Log the parsed data
            foreach (var unit in data.units)
            {
                Debug.Log($"Unit maxHealth: {unit.maxHealth}, damage: {unit.damage}");
            }
        }
    }
}