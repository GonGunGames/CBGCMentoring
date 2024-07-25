using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
    public static event Action OnBossDestroyed;

    private void OnDestroy()
    {
        // ������ �ı��� �� �̺�Ʈ�� �߻���ŵ�ϴ�.
        if (OnBossDestroyed != null)
        {
            OnBossDestroyed.Invoke();
        }
    }
}