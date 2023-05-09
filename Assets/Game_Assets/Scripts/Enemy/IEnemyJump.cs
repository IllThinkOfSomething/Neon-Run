using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public interface IEnemyJump
    {
        void Jump(Rigidbody2D rigidbody, float jumpForce);
    }

    public class EnemyJump : IEnemyJump
    {
        public void Jump(Rigidbody2D rigidbody, float jumpForce)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }
    }
}