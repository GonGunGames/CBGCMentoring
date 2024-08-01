using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.SearchService;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange(string str)
    {
        SceneManager.LoadScene(str);
    }
}