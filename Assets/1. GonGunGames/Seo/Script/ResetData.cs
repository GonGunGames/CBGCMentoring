using UnityEditor;
using UnityEngine;

public class ResetData : MonoBehaviour
{
    public DataPlayer dataPlayer;
    public DataInventory dataInventory;

    public void ResetBtn()
    {
        dataInventory.ResetAllData();
        dataPlayer.ResetAllData();
        DataPlayer.LoadData();
        EditorApplication.isPlaying = false;
    }
}