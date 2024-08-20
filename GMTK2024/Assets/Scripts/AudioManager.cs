using System;
using System.Collections;
using System.Collections.Generic;
using MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource, gameplaySource;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (MainMenuManager.instance == null) return;
        musicSource.volume = MainMenuManager.instance.musicVolume;
        gameplaySource.volume = MainMenuManager.instance.gameplayVolume;
        
    }

    public void PlayGameplaySFX(AudioClip clip)
    {
        gameplaySource.PlayOneShot(clip);
    }
}
