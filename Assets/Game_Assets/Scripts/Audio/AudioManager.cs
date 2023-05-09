using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {

        // Singleton
        public static AudioManager instance;

        public AudioSource mainMenuMusic;
        public AudioSource levelOneMusic;
        public AudioSource levelTwoMusic;
        public AudioSource levelThreeMusic;
        public AudioSource bossFight;

        public AudioSource[] sfx;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayMainMenuMusic()
        {
            // if accessed menu stop music
            levelOneMusic.Stop();
            levelTwoMusic.Stop();
            levelThreeMusic.Stop();
            
            mainMenuMusic.Play();
        }

        public void PlayLevelOneMusic()
        {
            // player can come from these levels
            mainMenuMusic.Stop();
            levelTwoMusic.Stop();
            
            levelOneMusic.Play();

        }
        
        public void PlayLevelTwoMusic()
        {
            // player can come from these levels
            levelOneMusic.Stop();
            levelThreeMusic.Stop();
            
            levelTwoMusic.Play();
        }

        public void PlayLevelThreeMusic()
        {
            mainMenuMusic.Stop();
            levelOneMusic.Stop();
            levelTwoMusic.Stop();
            bossFight.Stop();
            
            levelThreeMusic.Play();
        }
        
        public void PlayBossFightMusic()
        {
            // player can come from these levels
            levelOneMusic.Stop();
            levelTwoMusic.Stop();
            levelThreeMusic.Stop();
            
            bossFight.Play();
        }

        public void PlaySoundEffects(int sfxIndexToPlay)
        {
            // Stop if it is already playing, to avoid overlap of the same sound
            sfx[sfxIndexToPlay].Stop();
            sfx[sfxIndexToPlay].Play();
        }

        public void AdjustSfxPitch(int sfxIndexToAdjust)
        {
            sfx[sfxIndexToAdjust].pitch = Random.Range(.85f, 1.15f);
            PlaySoundEffects(sfxIndexToAdjust);
        }
    }
}
