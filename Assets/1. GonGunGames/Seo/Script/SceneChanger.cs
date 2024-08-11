using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange(string str)
    {
        SceneManager.LoadScene(str);
    }
}