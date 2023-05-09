using Game_Assets.Scripts.Ability;
using Game_Assets.Scripts.Bullet;
using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Transform _transform;
        public Animator animator;

        //Move
        private IPlayerMove _playerMove;
        public float movementSpeed;
        public bool ableToMove;

        // Jump
        private IPlayerJump _playerJump;
        public float jumpForce;
        
        //Shoot
        public BulletController bulletPrefab;
        public Transform shotStartPosition;

        
        
        //private bool _canDoubleJump;
        private IPlayerDash _playerDash;
        public float dashVelocity;
        public float dashDuration;
        public float dashCooldown;
        private float _dashActiveWaitTime;
        
        
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer afterImage;
        public float afterImageLifeSpan;
        public float inBetweenAfterImageDuration;
        private float _afterImageCounter;
        public Color afterImageColor;

        
        public Transform bombStartPosition;
        public GameObject bomb;
        public float bombCooldown;
        private float _bombActiveWaitTime;

        
        private PlayerAbilities _abilities;
        private static readonly int FiredShot = Animator.StringToHash("FiredShot");
        private static readonly int PutBomb = Animator.StringToHash("PutBomb");

        void Start() 
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _transform = GetComponent<Transform>();
            _playerMove = new PlayerMove(_rigidbody, animator, _transform);
            _playerMove.PlayerMovement = movementSpeed;
            _playerJump = new PlayerJump(_rigidbody, animator);
            _playerDash = new PlayerDash(_rigidbody, _transform, animator, gameObject);
            _abilities = GetComponent<PlayerAbilities>();
            ableToMove = true;
        }

        // Update is called once per frame
        void Update()
        {
            // if time scale 0 all movements shall be locked (pause screen)
            if (ableToMove && Time.timeScale != 0)
            {
                // player side-to-side movement
                _playerMove.Move(_playerMove.PlayerMovement);
            
                // player dash and after image effects.
                _playerDash.Dash(dashVelocity, dashDuration, dashCooldown, _abilities.dashAvailability, spriteRenderer,
                    afterImage, afterImageLifeSpan, inBetweenAfterImageDuration, afterImageColor);
            
                // Player lateral movement
                _playerJump.Jump(jumpForce, _abilities.doubleJumpAvailability);
                
                // shoot shots with left mouse button
                if (Input.GetButtonDown("Fire1")) 
                {
                    Instantiate(bulletPrefab, shotStartPosition.position, shotStartPosition.rotation).moveDirection = new Vector2(transform.localScale.x, 0f);
                    animator.SetTrigger(FiredShot);
                }

                if (_abilities.bombAvailability)
                {
                    if (_bombActiveWaitTime > 0)
                    {
                        _bombActiveWaitTime -= Time.deltaTime;
                    }
            
                    if (Input.GetButtonDown("Fire3"))
                    {
                 
                        if(_bombActiveWaitTime <= 0)
                        {
                            Instantiate(bomb, bombStartPosition.position, bombStartPosition.rotation);
                            animator.SetTrigger(PutBomb);
                            _bombActiveWaitTime = bombCooldown;
                        }
                 
                    }
                }
            }
            else
            {
                // Freeze player
                _rigidbody.velocity = Vector2.zero;
            }
        }
    }
}
