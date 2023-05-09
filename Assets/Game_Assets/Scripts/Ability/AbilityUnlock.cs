using Game_Assets.Scripts.Audio;
using UnityEngine;
using TMPro;

namespace Game_Assets.Scripts.Ability
{
    public class AbilityUnlock : MonoBehaviour
    {
        public bool unlockDoubleJump;
        public bool unlockDash;
        public bool unlockBombs;

        public GameObject pickupEffect;
        public string unlockedAbilityMessage;
        public TMP_Text unlockMessage;
        private void OnTriggerEnter2D(Collider2D col)
        {
            // object with tag "Player" can collide. Bug fix: Set idle sprite to Player tag 
            if (col.CompareTag("Player"))
            {
                // Find ability component on player
                var player = col.GetComponentInParent<PlayerAbilities>();
                
                // Unlock ability based on pickup
                
                if (unlockDoubleJump)
                    player.doubleJumpAvailability = true;
                
                if (unlockDash)
                    player.dashAvailability = true;
                
                if (unlockBombs)
                    player.bombAvailability = true;

                // Effect on pickup
                var transform1 = transform;
                Instantiate(pickupEffect, transform1.position, transform1.rotation);
                
                // Set canvas parent to null, otherwise on pickup the text will be destroyed instantly
                var parent = unlockMessage.transform.parent;
                parent.SetParent(null);
                // For some reason position gets across half of the inspector, so set position in place where pick up ability was
                parent.position = transform1.position;
                // Set text in TMP using the given string
                unlockMessage.text = unlockedAbilityMessage;
                // Activate TMP Object
                unlockMessage.gameObject.SetActive(true);
                // Destroy the unlock message
                Destroy(parent.gameObject, 5f);
                AudioManager.instance.PlaySoundEffects(6);
                // Destroy after pickup
                Destroy(gameObject);
            }
        }
        
    }
}
