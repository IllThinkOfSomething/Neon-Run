using System;
using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#pragma warning disable CS0414

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Game_Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        // Only one UI Controller shall be present in the project
        public static UIController instance;
        // Health bar slider
        public Slider healthSlider;

        
        // Black fade
        public Image blackFadeBetweenLevels;

        public float fadingSpeed;
        private bool _fadingBlack;
        private bool _fadingFromBlack;

        public string mainMenuScene;
        public GameObject pauseScreen;
        
        private void Awake()
        {
            // To save player's progress between scenes or on death
            if (instance == null)
            {
                instance = this;
                // On reload or next scene do not destroy the instance
                DontDestroyOnLoad(instance);
            }
            else
            {
                // Destroy game object if instance already exist, this avoid multiple player respawns
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // target 1f -> full black, 0f -> full white
            if (_fadingBlack)
            {
                blackFadeBetweenLevels.color = new Color(blackFadeBetweenLevels.color.r,
                    blackFadeBetweenLevels.color.g,
                    blackFadeBetweenLevels.color.b,
                    Mathf.MoveTowards(blackFadeBetweenLevels.color.a, 1f, fadingSpeed * Time.deltaTime)); // gradually move to 0 alpha value

                if (blackFadeBetweenLevels.color.a == 1f)
                {
                    _fadingBlack = false;
                }
            }
            else
            {
                blackFadeBetweenLevels.color = new Color(blackFadeBetweenLevels.color.r,
                    blackFadeBetweenLevels.color.g,
                    blackFadeBetweenLevels.color.b,
                    Mathf.MoveTowards(blackFadeBetweenLevels.color.a, 0f, fadingSpeed * Time.deltaTime)); // gradually move to 255 alpha value
                
                if (blackFadeBetweenLevels.color.a == 0f)
                {
                    _fadingFromBlack = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                Pause();
            }
        }

        public void UpdateHealthSlider(int currentPlayerHealth, int maxPlayerHealth)
        {
            healthSlider.maxValue = maxPlayerHealth;
            healthSlider.value = currentPlayerHealth;
        }

        public void StartFading()
        {
            _fadingBlack = true;
            _fadingFromBlack = false;
        }

        public void StartFadingFromBlack()
        {
            _fadingBlack = false;
            _fadingFromBlack = true;
        }
        
        // Pause/Unpause
        public void Pause()
        {
            AudioManager.instance.AdjustSfxPitch(13);
            if (!pauseScreen.activeSelf)
            {
                pauseScreen.SetActive(true);
                // Freeze time on pause
                Time.timeScale = 0f;
            }
            else
            {
                pauseScreen.SetActive(false);
                // Un-freeze time on pause
                Time.timeScale = 1f;
            }
        }

        public void MainMenu()
        {
            Time.timeScale = 0f;
            
            // Destroy and disable singleton instances alongside the dontDestroyObjects
            Destroy(PlayerHealthController.instance.GetComponent<PlayerController>().gameObject);
            // Destroy(PlayerHealthController.instance);
            // PlayerHealthController.instance = null;
            
            Destroy(PlayerRespawnController.instance.gameObject);
            PlayerRespawnController.instance = null;
            
            
            instance = null;
            Destroy(gameObject);
            
            SceneManager.LoadScene(mainMenuScene);
        }

        public void QuitApplication()
        {
            Application.Quit();
            Debug.Log("Quit");
            
            
        }
    }
}
