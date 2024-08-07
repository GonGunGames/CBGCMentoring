using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource backgoundMusic;
    AudioSource buttonSound;

    public Slider bgmSlider;
    public Slider btnSlider;


    private void Awake()
    {
        backgoundMusic = GetComponent<AudioSource>();
        buttonSound = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
