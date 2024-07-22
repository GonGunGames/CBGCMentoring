using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditorInternal;

namespace AllUnits
{

    [System.Serializable]
    public class GameData
    {
        public string name;

        public float maxHealth;
        public float damage;
        public float currentHealth;
        public bool isDamage;
    }


    [System.Serializable]
    public class Unit : MonoBehaviour
    {
        public GameData gameData;

        [ContextMenu("From Json Data")]
        void LoadGameDataFromJson()
        {
            string path = Path.Combine(Application.persistentDataPath, "gamerData.json");

            string jsonData = File.ReadAllText(path);

            gameData = JsonUtility.FromJson<GameData>(jsonData);

        }

        private void Start()
        {
            LoadGameDataFromJson();
            InitializeHealth();
        }

        private void InitializeHealth()
        {
            gameData.currentHealth = gameData.maxHealth - gameData.damage;
        }
    }
}