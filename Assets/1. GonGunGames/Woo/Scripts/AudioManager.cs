using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Bgm;
    public AudioSource BossBgm;
    // Start is called before the first frame update
    void Start()
    {
        Bgm.Play();
    }

    public void SpawnBoss()
    {
        Bgm.Stop();
        BossBgm.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
