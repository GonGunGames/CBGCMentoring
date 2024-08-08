using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public void OnClick()
    {
        SoundManager.Instance.PlayClickSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
