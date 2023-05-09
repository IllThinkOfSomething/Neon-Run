using System;
using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public class PlayerDamage : MonoBehaviour
    {
        public int damageAmount = 1;

        private void OnCollisionEnter2D(Collision2D col)
        {
            // Collision does not know about collider that is why we get tag from gameObject.
            if (col.gameObject.CompareTag("Player"))
            {
               InflictDamage(); 
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                InflictDamage();
            }
        }
        
        void InflictDamage()
        {
            PlayerHealthController.instance.DamagePlayer(damageAmount);
        }
    }
}
