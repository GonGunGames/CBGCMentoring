using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
    public static event Action OnBossDestroyed;
    public AudioSource audioSource;
    private AudioManager audioManager;

    private void Awake()
    {
        // AudioSource 컴포넌트를 가져옵니다.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is not assigned or not found.");
        }

        // AudioManager 인스턴스를 찾아서 참조합니다.
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }

    private void Start()
    {
        if (audioManager != null)
        {
            audioManager.SpawnBoss();
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned. Audio will not play.");
        }
    }

    private void OnDestroy()
    {
        // 보스가 파괴될 때 이벤트를 발생시킵니다.
        OnBossDestroyed?.Invoke();
    }
}