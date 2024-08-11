using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveToggle : MonoBehaviour
{
    public GameObject targetObject;
    public void SetActiveToggleFunc()
    {
        if (targetObject.activeSelf == false)
            targetObject.SetActive(true);
        else
            targetObject.SetActive(false);
    }
}
