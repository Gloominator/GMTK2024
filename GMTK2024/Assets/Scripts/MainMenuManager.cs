using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        
        // SINGLETON
        public static MainMenuManager instance;
        
        // UI Elements
        [SerializeField] private GameObject mainMenuObject, optionsMenuObject, creditsMenuObject;
        [SerializeField] private GameObject gameplaySubMenu, videoSubMenu, audioSubMenu;
        [SerializeField] private Slider musicVolSlider, gameplayVolSlider;
        [SerializeField] private Toggle muteVolToggle;
        [SerializeField] private Toggle fullscreenToggle;

        
        // SETTING VARIABLES
        public float musicVolume, gameplayVolume;
        public bool muteVol;
        public bool fullscreen;
        public int screenTypeState; // 0 = Fullscreen, 1 = Borderless Window, 2 = Windowed
        private ScreenTypes screenState;
        private GameObject currentMenu;
        private GameObject currentSubMenu;
        
        
        // UNITY FUNCTIONS
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            currentMenu = mainMenuObject;
            currentSubMenu = gameplaySubMenu;
        }

        private void Update()
        {
            UpdateVolumeValues();
        }

        // GAME STATE FUNCTIONS
        public void StartGame()
        {
            Debug.Log("Game Start");
            SceneManager.LoadScene("Main-UIRework");

        }
        
        public void QuitGame()
        {
            Debug.Log("Game Quit");
            Application.Quit();
        }
        

        // MENU SWAPPING FUNCTIONS
        public void SwapMainMenu(GameObject newMenu)
        {
            SwapMenu(currentMenu, newMenu);
            currentMenu = newMenu;
        }
        
        public void SwapSubMenu(GameObject newMenu)
        {
            SwapMenu(currentSubMenu, newMenu);
            currentSubMenu = newMenu;
        }
        
        
        // OPTIONS MENU FUNCTIONS
        //  - AUDIO SETTINGS
        private void UpdateVolumeValues()
        {
            muteVol = muteVolToggle.isOn;
            musicVolume = musicVolSlider.value;
            gameplayVolume = gameplayVolSlider.value;
        }

        
        //  - VIDEO SETTINGS
        public void SwapScreenType()
        {
            switch (screenTypeState)
            {
                case 0:
                    screenState = ScreenTypes.Fullscreen;
                    break;
                case 1:
                    screenState = ScreenTypes.WindowedBorderless;
                    break;
                case 2:
                    screenState = ScreenTypes.Window;
                    break;
                default:
                    Debug.LogError("This is not a valid screen type. Check your settings.");
                    break;
                    
            }
        }

        public void ToggleFullscreen()
        {
            fullscreen = fullscreenToggle.isOn;
            Screen.fullScreen = fullscreen;
        }
        
        // TODO: Implement Later
        public void ChangeResolution()
        {
            
        }
        
        
        //  - GAMEPLAY SETTINGS
        
        
        
        
        // HELPER FUNCTIONS
        private void SwapMenu(GameObject oldMenu, GameObject newMenu)
        {
            oldMenu.SetActive(false);
            newMenu.SetActive(true);
        }
    }
    
    public enum ScreenTypes 
    {
        Fullscreen,
        WindowedBorderless,
        Window
    }
    
}
