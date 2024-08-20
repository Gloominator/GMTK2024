using System;
using System.Collections;
using System.Collections.Generic;
using MainMenu;
using UnityEngine;

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
        musicSource.volume = MainMenuManager.instance.musicVolume;
        gameplaySource.volume = MainMenuManager.instance.gameplayVolume;
        
    }
}
