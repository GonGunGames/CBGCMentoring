using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemViewToggle : MonoBehaviour
{
    private GameObject targetObject; // 하이어라키에서 활성/비활성화할 객체
    private Toggle toggle; // 프리팹의 토글 컴포넌트

    public GameObject equipButton;
    public GameObject changeButton;
    GameObject button;

    Transform childTransform;

    void Awake()
    {
        // 현재 게임 오브젝트에서 Toggle 컴포넌트를 가져옴
        toggle = GetComponent<Toggle>();
        toggle.isOn = false;
        targetObject = GameObject.FindWithTag("ItemView");
        equipButton = GameObject.FindGameObjectWithTag("EquipButton");
        changeButton = GameObject.FindGameObjectWithTag("ChangeButton");
    }

    void Start()
    {
        targetObject.SetActive(false);
        // 토글의 OnValueChanged 이벤트에 메서드 추가
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

    }

    // 토글 상태가 변경될 때 호출되는 메서드
    void OnToggleValueChanged(bool isOn)
    {
        //Debug.Log(isOn);
        // 하이어라키 객체 활성/비활성화
        if (targetObject != null && isOn == true)
        {
            targetObject.SetActive(isOn);
            
            if (transform.childCount != 7)
            {
                childTransform = transform.GetChild(4);
                if (childTransform.name != "Item(Clone)")
                {
                    targetObject.gameObject.SetActive(false);

                }

            }

            if (equipButton != null && transform.parent.parent.name == "EquipSlot")
            {
                equipButton.gameObject.SetActive(false);
            }

            if (changeButton != null && transform.parent.parent.name == "EquipSlot")
            {
                changeButton.gameObject.SetActive(false);
            }

            

        }
        else
        {
            if (equipButton.activeSelf == false)
            {
                equipButton.gameObject.SetActive(true);
            }
            if (changeButton.activeSelf == false)
            {
                changeButton.gameObject.SetActive(true);
            }
        }

    }
}