using UnityEngine;
using UnityEngine.UI;

public class WelcomPackage : MonoBehaviour
{
    public GameObject targetObject;
    public ItemBase.ItemData[] item;
    public DataPlayer dataPlayer;
    public Slider progressBar; // Unity UI의 Slider를 활용한 진행 바

    void Start()
    {
        if (dataPlayer.baseStats[(int)StatType.Login].value == 0)
        {
            targetObject.SetActive(true);
            InventoryManager.Instance.AddAmountOfItem(item[0], 1, 100);
            UpdateProgressBar(0.25f);
            InventoryManager.Instance.AddAmountOfItem(item[1], 1, 111);
            UpdateProgressBar(0.50f);
            InventoryManager.Instance.AddAmountOfItem(item[2], 1, 114);
            UpdateProgressBar(0.75f);
            InventoryManager.Instance.AddAmountOfItem(item[3], 1, 117);
            UpdateProgressBar(1.0f);
            dataPlayer.baseStats[(int)StatType.Login].value++;
        }
        else
        {
            targetObject.SetActive(false);
        }
    }

    void UpdateProgressBar(float progress)
    {
        if (progressBar != null)
        {
            progressBar.value = progress;
        }
        else
        {
            Debug.Log("Progress: " + (progress * 100) + "%");
        }
    }

    public void Onclick()
    {
        targetObject.SetActive(false);
    }

    void Update()
    {
        // Update logic
    }
}
