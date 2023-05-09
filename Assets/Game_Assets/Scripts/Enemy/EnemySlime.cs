using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public class EnemySlime : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        public Animator animator;
        public Transform[] boundaryPoints;
        private int _currentBoundary;

        public float movementSpeed;
        public float timeToWaitAtPoint;
        private float _activeWaitTime;
        public float jumpForce;

        private IEnemyMoving _enemyMoving;
        private IEnemyWait _enemyWait;
        private IEnemyJump _enemyJump;
        private EnemyHealthController _enemyHealthController;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jump");

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _activeWaitTime = timeToWaitAtPoint;
            _enemyMoving = new EnemyMoving();
            _enemyWait = new EnemyWait();
            _enemyJump = new EnemyJump();
            
            // work around for boundaries to stop moving along-side enemy
            foreach (var boundary in boundaryPoints)
            {
                boundary.SetParent(null);
            }
        }

        // Update is called once per frame
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
            
            // For some reason the sprite Transform object starts at 1.9 more y axis due to its size. that is why we take away 1.9 from y axis. WHEN SCALE IS 12x12x12
            if (transform.position.y - 1f < boundaryPoints[_currentBoundary].position.y - 0.5f && _rigidbody.velocity.y < 0.1f)
            {
                _enemyJump.Jump(_rigidbody, jumpForce);
                animator.SetTrigger(Jump);
            }

            //Debug.Log(_rigidbody.velocity); //Test purposes
            
            animator.SetFloat(Speed, Mathf.Abs(_rigidbody.velocity.x));
        }
        
    }
}
