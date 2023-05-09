using System;
using Game_Assets.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Assets.Scripts.Enemy
{
    public class BossHealthController : MonoBehaviour
    {
        // There  will be only one BossHealthController active at once.
        public static BossHealthController instance;

        public Slider bossHealthBar;
        public int bossCurrentHp;
        public BossBattle boss;
        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set boss health bar values
            bossHealthBar.maxValue = bossCurrentHp;
            // Since boss will not heal, there is no point in max health field
            bossHealthBar.value = bossCurrentHp;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void BossTakeDamage(int damageAmount)
        {
            bossCurrentHp -= damageAmount;

            // if boss ran out of hp
            if (bossCurrentHp <= 0)
            {
                bossCurrentHp = 0;
                
                boss.EndBattle();
                
                // Death of boss sfx
                AudioManager.instance.PlaySoundEffects(1);
            }
            else
                AudioManager.instance.PlaySoundEffects(2);
            
            // Update UI of health bar
            bossHealthBar.value = bossCurrentHp;
        }
    }
}
