using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public interface IEnemyWait
    {
        (float,int) Wait(Rigidbody2D rigidbody, float activeWaitTime, float timeToWaitAtPoint, int currentBoundary,
            float boundaryPointCount);
    }

    public class EnemyWait : IEnemyWait
    {
        public (float,int) Wait(Rigidbody2D rigidbody, float activeWaitTime, float timeToWaitAtPoint, int currentBoundary,
            float boundaryPointCount)
        {
            rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
            activeWaitTime -= Time.deltaTime;
            if (activeWaitTime <= 0)
            {
                // Reset timer
                activeWaitTime = timeToWaitAtPoint;

                //Switch to next boundary point
                currentBoundary++;

                // if last point was reached, reset
                if (boundaryPointCount <= currentBoundary)
                    currentBoundary = 0;
            }
            
            return (activeWaitTime, currentBoundary);
        }
    }
}