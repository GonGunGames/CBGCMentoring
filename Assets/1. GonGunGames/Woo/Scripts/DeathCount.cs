using System.Collections;
using System.Collections.Generic;
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
        if (deathCountText != null)
        {
            deathCountText.text = ": 0" + deathCount.ToString();
        }
    }
}