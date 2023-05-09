using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public interface IPlayerDash
    {
        void Dash(float dashVelocity, float dashDuration, float dashCooldown, bool dashAvailability,
            SpriteRenderer spriteRenderer, SpriteRenderer afterImage, float afterImageLifeSpan,
            float inBetweenAfterImageDuration, Color color);
        
        void AfterImage(SpriteRenderer spriteRenderer, SpriteRenderer afterImage, float afterImageLifeSpan, float inBetweenAfterImageDuration, Color afterImageColor);

    }
}