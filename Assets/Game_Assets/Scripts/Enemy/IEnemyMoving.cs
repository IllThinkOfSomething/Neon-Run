using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public interface IEnemyMoving
    {
        void EnemyMove(float movementSpeed, Rigidbody2D rigidbody);
    }

    public class EnemyMoving : IEnemyMoving
    {
        public void EnemyMove(float movementSpeed, Rigidbody2D rigidbody)
        {
            //For direction
            var vector = new Vector3(1f, 1f, 1f);
            if(movementSpeed < 0)
                vector = new Vector3(-1f, 1f, 1f);
            
            //Movement
            rigidbody.velocity = new Vector2(movementSpeed, rigidbody.velocity.y);
            // Flip the enemy
            rigidbody.transform.localScale = vector;
        }
    }
    
}