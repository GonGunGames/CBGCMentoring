using UnityEngine;
using System;

public class Ellite : MonoBehaviour
{
    public static event Action OnElliteDestroyed;

    private void OnDestroy()
    {
        // 보스가 파괴될 때 이벤트를 발생시킵니다.
        if (OnElliteDestroyed != null)
        {
            OnElliteDestroyed.Invoke();
        }
    }
}