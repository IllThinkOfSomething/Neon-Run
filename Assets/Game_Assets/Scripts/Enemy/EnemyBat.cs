using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public class EnemyBat : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        public Animator animator;
        public Transform[] boundaryPoints;
        private int _currentBoundary;

        public float movementSpeed;
        public float timeToWaitAtPoint;
        private float _activeWaitTime;

        private IEnemyMoving _enemyMoving;
        private IEnemyWait _enemyWait;
        private EnemyHealthController _enemyHealthController;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _activeWaitTime = timeToWaitAtPoint;
            _enemyMoving = new EnemyMoving();
            _enemyWait = new EnemyWait();

            // work around for boundaries to stop moving along-side enemy
            foreach (var boundary in boundaryPoints)
            {
                boundary.SetParent(null);
            }
        }

        void Update()
        {
            // check if absolute distance value is more than 0.2 from boundary point
            if (Mathf.Abs(transform.position.x - boundaryPoints[_currentBoundary].position.x) > 1f)
            {
                if (transform.position.x < boundaryPoints[_currentBoundary].position.x)
                {
                    // Move to the right to reach boundary
                    _enemyMoving.EnemyMove(movementSpeed, _rigidbody);
                }
                else
                {
                    // Move to the left to reach boundary
                    _enemyMoving.EnemyMove(-movementSpeed, _rigidbody);
                }
            }
            else
                (_activeWaitTime, _currentBoundary) = _enemyWait.Wait(_rigidbody, _activeWaitTime, timeToWaitAtPoint, _currentBoundary, boundaryPoints.Length); // At boundary point wait
        }
    }
}