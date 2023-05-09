using System;
using Game_Assets.Scripts.Audio;
using JetBrains.Annotations;
using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public class EnemyHealthController : MonoBehaviour
    {
        public int totalHealth;
        private readonly float _timeTillDestroyed = 1;
        [CanBeNull] public Animator animator;
        public GameObject deathEffect;
        private static readonly int EnemyHealth = Animator.StringToHash("EnemyHealth");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int IsDead = Animator.StringToHash("isDead");

        public void DamageEnemy(int damageAmount)
        {
            totalHealth -= damageAmount;
            AudioManager.instance.AdjustSfxPitch(9);
            if (animator != null)
            {
                animator.SetTrigger(Hit);
            }

            if (totalHealth <= 0)
            {
                if (deathEffect != null)
                {
                    var transform1 = transform;
                    Instantiate(deathEffect, transform1.position, transform1.rotation);
                }
                if(animator != null)
                    animator.SetBool(IsDead, true);
                AudioManager.instance.PlaySoundEffects(4);
                Destroy(gameObject, _timeTillDestroyed);
            }
        }
        
    }
}
