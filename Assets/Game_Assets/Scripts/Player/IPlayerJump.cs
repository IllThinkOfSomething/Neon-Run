using Game_Assets.Scripts.Audio;
using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public interface IPlayerJump
    {
        void Jump(float jumpForce, bool doubleJumpAvailability);
    }
    
    public class PlayerJump: IPlayerJump
    {
        private readonly Animator _animator;
        private readonly Rigidbody2D _rigidbody;
        private bool _isGrounded;
        private bool _doubleJumpAvailability;
        
        public readonly Transform groundCheck;
        public LayerMask groundLayers;

        public PlayerJump(Rigidbody2D rigidbody, Animator animator)
        {
            _rigidbody = rigidbody;
            _animator = animator;
            groundCheck = GameObject.Find("GroundCheck").transform;
            groundLayers = LayerMask.GetMask("Ground", "Destructable");
        }

        public void Jump(float jumpForce, bool doubleJumpAvailability)
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayers);
            

            if (Input.GetButtonDown("Jump") && (_isGrounded || (_doubleJumpAvailability && doubleJumpAvailability)))
            {
                if (_isGrounded)
                {
                    _doubleJumpAvailability = true;
                    AudioManager.instance.AdjustSfxPitch(10);
                }
                else
                {
                    AudioManager.instance.AdjustSfxPitch(10);
                    _doubleJumpAvailability = false;
                    _animator.SetTrigger("DoubleJump");
                }

                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
            }
            _animator.SetBool("isGround", _isGrounded);
        }
    }
    }