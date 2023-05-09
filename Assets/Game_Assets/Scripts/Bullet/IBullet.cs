using Game_Assets.Scripts.Audio;
using UnityEngine;

namespace Game_Assets.Scripts.Bullet
{
  public interface IBullet
  {
    void Shoot(Vector2 moveDirection, float bulletVelocity);
    void ExplosionOnImpact(GameObject particles);
  }

  public class Bullet : IBullet
  {
    private readonly Rigidbody2D _rigidbody;
    private readonly Transform _transform;
    private readonly Transform _shotStartPosition;
    private readonly Animator _animator;
    private readonly GameObject _gameObject; //For instantiate and destroy methods

    

    public Bullet(Rigidbody2D rigidbody, Transform transform, GameObject gameObject)
    {
      _rigidbody = rigidbody;
      _transform = transform;
      _gameObject = gameObject;
      _shotStartPosition = GameObject.Find("ShotStartPosition").transform;
    }
    public void Shoot(Vector2 moveDirection, float bulletVelocity)
    {
      AudioManager.instance.PlaySoundEffects(2);
      _rigidbody.velocity = moveDirection * bulletVelocity;
    }

    public void ExplosionOnImpact(GameObject particles)
    {
      if (particles != null)
        Object.Instantiate(particles, _transform.position, Quaternion.identity); // Quaternion.identity -> rotation
      // bullet impact sound effect
      AudioManager.instance.PlaySoundEffects(3);
      Object.Destroy(_gameObject);
    }
  }
}