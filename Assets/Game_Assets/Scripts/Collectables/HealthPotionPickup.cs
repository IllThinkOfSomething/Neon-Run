using System;
using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.Player;
using UnityEngine;

namespace Game_Assets.Scripts.Collectables
{
    public class HealthPotionPickup : MonoBehaviour
    {
        public int healthPercentage;

        public GameObject healthPickupEffect;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                // Heal player
                var amount = MathF.Round((float)PlayerHealthController.instance.maxHealth / 100 * healthPercentage);
                PlayerHealthController.instance.
                    HealPlayerByPickup((int)amount);

                if (healthPickupEffect != null)
                {
                    var transform1 = transform;
                    Instantiate(healthPickupEffect, transform1.position, transform1.rotation);
                    
                }
                AudioManager.instance.PlaySoundEffects(6);
                Destroy(gameObject);
            }
        }
    }
}
