using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    int stage;

    public Button preButton;
    public Button nextButton;

    // Start is called before the first frame update
    void Start()
    {
        // GetStageInfo
        stage = 1;
        stageText.text = stage.ToString();


        preButton.onClick.AddListener(() =>
        {
            StageChange(-1);
        });
        nextButton.onClick.AddListener(() =>
        {
            StageChange(1);
        });
    }

    public void StageChange(int value)
    {
        if (stage == 1 && value == -1)
            return;
        else if (stage == 10 && value == 1)
            return;

        stage += value;
        stageText.text = stage.ToString();
    }

}
