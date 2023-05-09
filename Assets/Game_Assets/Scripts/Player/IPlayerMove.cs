using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public interface IPlayerMove
    {
        public float PlayerMovement { get; set; }
        void Move(float movementSpeed);
    }

    public class PlayerMove : IPlayerMove
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Animator _animator;
        private readonly Transform _transform;
        private IPlayerDash _dash;
        public float PlayerMovement { get; set; }

        public PlayerMove(Rigidbody2D rigidbody, Animator animator, Transform transform)
        {
            _rigidbody = rigidbody;
            _animator = animator;
            _transform = transform;
        }

        

        public void Move(float movementSpeed)
        {
            
             var horizontalInput = Input.GetAxis("Horizontal");
             _rigidbody.velocity = new Vector2(horizontalInput * movementSpeed, _rigidbody.velocity.y);
             
             // Flip character corresponding with velocity.
             if (_rigidbody.velocity.x < 0)
             {
                 _transform.localScale = new Vector3(-1f, 1f, 1f);
             }
             else if (_rigidbody.velocity.x > 0)
             {
                 _transform.localScale = new Vector3(1f, 1f, 1f);
             }
             _animator.SetFloat("speed", Mathf.Abs(_rigidbody.velocity.x)); // absolute value solves the movement to left issue.


        }
    }
}