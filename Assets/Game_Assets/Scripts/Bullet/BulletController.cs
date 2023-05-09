using Game_Assets.Scripts.Enemy;
using UnityEngine;

namespace Game_Assets.Scripts.Bullet
{
    public class BulletController : MonoBehaviour
    {
        private IBullet _bullet;
        public float bulletVelocity;
        public Vector2 moveDirection; 
        private Rigidbody2D _rigidbody;
        public GameObject particlesEffectOnImpact;

        public int damageAmount;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _bullet = new Bullet(_rigidbody, transform, gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            _bullet.Shoot(moveDirection ,bulletVelocity);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                // Damage enemy
                col.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
            }

            if (col.CompareTag("Boss"))
            {
                BossHealthController.instance.BossTakeDamage(damageAmount);
            }
            _bullet.ExplosionOnImpact(particlesEffectOnImpact);
        }

        // if camera can no longer see the bullet it should be destroyed
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

    }
}
