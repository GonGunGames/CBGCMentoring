using UnityEngine;
using UnityEngine.UI;

public class GoldCountManager : MonoBehaviour
{
    public Text totalGold;  // Total Gold UI Text

    private int goldCount;  // Total gold count

    // Singleton instance
    public static GoldCountManager Instance;

    private void Awake()
    {
        // Ensure there's only one instance of GoldCountManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: keep this object between scenes
        }
        else
        {
            Destroy(gameObject);  // Ensure there's only one instance
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize gold count and update the text
        goldCount = 0;
        UpdateGoldText();
    }

    // Method to increment the gold count and update the UI text
    public void IncrementGoldCount(int amount)
    {
        goldCount += amount;
        UpdateGoldText();
    }

    // Method to update the UI text with the current gold count
    private void UpdateGoldText()
    {
        if (totalGold != null)
        {
            totalGold.text = "Gold: " + goldCount.ToString();
        }
    }
}