using System;
using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.Player;
using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public class BossProjectile : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        public float projectileSpeed;
        public int projectileDamageAmount;
        public GameObject effectOnImpact;
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            var direction = transform.position - Player.PlayerHealthController.instance.transform.position;
            // rotate based on players position and convert to degrees
            var movementAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // set rotation angle of the bullet to face player
            transform.rotation = Quaternion.AngleAxis(movementAngle, Vector3.forward);
            // Boss shooting sound
            AudioManager.instance.AdjustSfxPitch(2);
        }

        // Update is called once per frame
        void Update()
        {
            // due to rigidbody calculation delta time is not needed here ( unity will calculate how much it should move per frame)
            _rigidbody.velocity =transform.right * (-1 * projectileSpeed);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            // damage player
            if(col.gameObject.CompareTag("Player"))
                PlayerHealthController.instance.DamagePlayer(projectileDamageAmount);
            
            // use effect
            if(effectOnImpact != null)
            {
                var transform1 = transform;
                Instantiate(effectOnImpact, transform1.position, transform1.rotation);
            }
            
            // Bullet sound effect
            AudioManager.instance.PlaySoundEffects(3);
            // Destroy projectile
            Destroy(gameObject);
        }
    }
}
