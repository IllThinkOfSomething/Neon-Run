using Game_Assets.Scripts.Player;
using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public class EnemyFlyingController : MonoBehaviour
    {
        public float inRangeToFollow;
        private bool _isFollowing;

        public float movementSpeed;
        public float turnSpeed;

        private Transform _playerInstance;
        
        // Start is called before the first frame update
        void Start()
        {
            // Take transform from singleton method.
            _playerInstance = PlayerHealthController.instance.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isFollowing)
            {
                // Distance between two points(player and flying enemy) 
                if (Vector3.Distance(transform.position, _playerInstance.position) < inRangeToFollow)
                    _isFollowing = true;
            }
            else
            {
                // if player dies the enemy should not follow
                if (_playerInstance.gameObject.activeSelf)
                {
                    var direction = transform.position - _playerInstance.position;
                    // Set flying enemy rotation direction towards player
                    var movementAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    // Wanted rotation for 3rd vector value
                    var targetRotation = Quaternion.AngleAxis(movementAngle, Vector3.forward);
                    // Slerp -> starting rotation position, wanted rotation position, amount of time for it to happen. Makes the object move from one rotation to another
                    Transform transform1;
                    (transform1 = transform).rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                    // follow player (Time.delta time solves the higher/lower fps speed inconsistencies)
                    transform1.position += transform1.right * (-1 * (movementSpeed * Time.deltaTime));
                }
            }
        }
    }
}
