using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Backgound")]
    public AudioClip backgoundMusic;

    [Header("UI_Button")]
    public AudioClip buttonClickSound;

    AudioSource backgoundMusicSource;
    AudioSource buttonSoundSource;

    public Slider bgmSlider;
    public Slider btnSlider;


    private void Awake()
    {
        backgoundMusicSource = gameObject.AddComponent<AudioSource>();
        buttonSoundSource = gameObject.AddComponent<AudioSource>();

        backgoundMusicSource.loop = true;
    }

    private void Start()
    {
        backgoundMusicSource.clip = backgoundMusic;
        buttonSoundSource.clip = buttonClickSound;
        backgoundMusicSource.Play();

        
    }

    public void SetMusicVolume()
    {
        backgoundMusicSource.volume = bgmSlider.value;
    }

    public void SetButtonVolume()
    {
        buttonSoundSource.volume = btnSlider.value;
    }


    public void PlayClickSound()
    {
        buttonSoundSource.PlayOneShot(buttonClickSound);
    }
}
