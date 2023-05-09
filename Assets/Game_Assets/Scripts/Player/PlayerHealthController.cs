using System;
using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.UI;
using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        // Singleton (since there will not be anymore players)
        public static PlayerHealthController instance;
        
        public int currentHealth;
        public int maxHealth;

        public float invincibilityPeriod;
        private float _invincibilityActiveTimeLeft;
        public float flashingPeriod;
        private float _flashingActiveTimeLeft;

        public SpriteRenderer playerSprite;
        
        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            UIController.instance.UpdateHealthSlider(currentHealth, maxHealth);
        }
        
        // due to respawns
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
            if (_invincibilityActiveTimeLeft > 0)
            {
                _invincibilityActiveTimeLeft -= Time.deltaTime;

                _flashingActiveTimeLeft -= Time.deltaTime;
                if (_flashingActiveTimeLeft <= 0)
                {
                    // flashing effect 
                    playerSprite.enabled = !playerSprite.enabled;
                    _flashingActiveTimeLeft = flashingPeriod;
                }

                
            }

            if (_invincibilityActiveTimeLeft <= 0)
            {
                // To avoid staying invisible after active invincibility time frame ends.
                playerSprite.enabled = true;
                // to avoid going into negative value after it ends.
                _flashingActiveTimeLeft = 0f;
            }
        }


        public void DamagePlayer(int damageAmount)
        {
            
            if (_invincibilityActiveTimeLeft <= 0)
            {
                currentHealth -= damageAmount;
                
                if (currentHealth <= 0)
                {
                    // player should not be able to reach less than 0 health
                    currentHealth = 0;
                    AudioManager.instance.PlaySoundEffects(7);
                    //gameObject.SetActive(false);
                    PlayerRespawnController.instance.Reload();
                }
                else
                {
                    AudioManager.instance.AdjustSfxPitch(8);
                    _invincibilityActiveTimeLeft = invincibilityPeriod;

                }

                
                UIController.instance.UpdateHealthSlider(currentHealth, maxHealth);
            }

            
        }

        public void ResetHealth()
        {
            // After death reset health
            currentHealth = maxHealth;
            
            // Update UI health bar
            UIController.instance.UpdateHealthSlider(currentHealth, maxHealth);
        }

        public void HealPlayerByPickup(int healthAmount)
        {
            // Add health potion amount and stop exceed max health
            currentHealth += healthAmount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            
            // Update UI health bar
            UIController.instance.UpdateHealthSlider(currentHealth, maxHealth);
        }
    }
}
