using UnityEngine;
using UnityEngine.UI;

public class DeathCount : MonoBehaviour
{
    public static DeathCount Instance;  // Singleton instance
    public Text deathCountText;  // UI Text to display the death count

    private int deathCount = 0;  // Current death count

    private void Awake()
    {
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

    public void IncrementDeathCount()
    {
        deathCount++;
        UpdateDeathCountUI();
    }

    private void UpdateDeathCountUI()
    {
        if (deathCountText != null)
        {
            deathCountText.text = "" + deathCount.ToString();
        }
    }

    public int GetDeathCount()
    {
        return deathCount;
    }
}