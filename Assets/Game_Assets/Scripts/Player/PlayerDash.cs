using Game_Assets.Scripts.Audio;
using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public class PlayerDash: IPlayerDash
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Animator _animator;
        private readonly Transform _transform;
        private readonly GameObject _gameObject;
        private readonly IPlayerMove _playerMove;
        private float _playerMovementSpeed;
        
        private float _dashCooldownLeftTime;
        private float _dashInProgressTime;

        private float _afterImageCounter;
        public PlayerDash(Rigidbody2D rigidbody, Transform transform, Animator animator, GameObject gameObject)
        {
            _rigidbody = rigidbody;
            _transform = transform;
            _animator = animator;
            _gameObject = gameObject;
            _playerMove = new PlayerMove(_rigidbody, _animator, _transform);
            _playerMovementSpeed = _playerMove.PlayerMovement;
            
            
        }

        public void Dash(float dashVelocity, float dashDuration, float dashCooldown, bool dashAvailability,
            SpriteRenderer spriteRenderer, SpriteRenderer afterImage, float afterImageLifeSpan,
            float inBetweenAfterImageDuration, Color afterImageColor)
        {
            if (dashAvailability)
            {
                // Dash Power-Up
                if (Input.GetButtonDown("Fire2") && _dashCooldownLeftTime < 0)
                {
                    AudioManager.instance.AdjustSfxPitch(13);
                    _dashInProgressTime = dashDuration;
                    AfterImage(spriteRenderer, afterImage, afterImageLifeSpan, inBetweenAfterImageDuration, afterImageColor); // Dash effects
                }
                else
                {
                    _dashCooldownLeftTime -= Time.deltaTime;
                }


                if (_dashInProgressTime > 0)
                {
                    // Fixes a problem between 60fps and 240fps
                    _dashInProgressTime -= Time.deltaTime; // delta time is a duration of each update 
                    // Dash speed and direction
                    
                    _rigidbody.velocity = new Vector2(dashVelocity * _transform.localScale.x, _rigidbody.velocity.y);

                    // Create a trail of after images
                    _afterImageCounter -= Time.deltaTime;
                    if (_afterImageCounter <= 0)
                    {
                        AfterImage(spriteRenderer, afterImage, afterImageLifeSpan, inBetweenAfterImageDuration, afterImageColor);
                    }
                   

                    // activate cooldown
                    _dashCooldownLeftTime = dashCooldown;

                }
            }
        }
        
        public void AfterImage(SpriteRenderer spriteRenderer, SpriteRenderer afterImage, float afterImageLifeSpan, float inBetweenAfterImageDuration, Color afterImageColor)
        {

            var image = Object.Instantiate(afterImage, _transform.position, _transform.rotation);
            image.sprite = spriteRenderer.sprite; //Current player sprite
            image.transform.localScale = _transform.localScale * 6; // direction (original player scale is too small) 
            image.color = afterImageColor; // color of after image effect

            Object.Destroy(image.gameObject, afterImageLifeSpan); // Destroy effect after its lifespan
            _afterImageCounter = inBetweenAfterImageDuration;
            
        }
        
    }
}