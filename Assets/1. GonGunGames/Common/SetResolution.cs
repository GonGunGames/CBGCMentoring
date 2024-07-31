using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    void Awake()
    {
        SetResolutionFHD();
    }
    public void SetResolutionFHD()
    {
        int setWidth = (int)(1080 * 0.5f); // 화면 너비
        int setHeight = (int)(1920 * 0.5f); // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, false);
    }
}
