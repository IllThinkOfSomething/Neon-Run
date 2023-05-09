using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Assets.Scripts.Camera
{
    public class CameraController : MonoBehaviour, ICamera
    {
        private PlayerController _playerController;
        public BoxCollider2D cameraBounds;

        private float _orthographicHeight;
        private float _orthographicWidth;
        private bool _isPlayerControllerNotNull;

        // To avoid multiple calls of FindObjectOfType
        private void Start()
        {
            _isPlayerControllerNotNull = _playerController != null;
            PlayLevelMusic(SceneManager.GetActiveScene().name);
            // AudioManager.instance.PlayLevelOneMusic();
        }

        private void Awake()
        {
            // Access through singleton, otherwise player controller instance will be tried to access after it is destroyed.
            // Setup camera's perspective
            var main = UnityEngine.Camera.main;
        
            _orthographicHeight = main!.orthographicSize;
            _orthographicWidth = _orthographicHeight * main.aspect;
            
            _playerController = PlayerHealthController.instance.GetComponent<PlayerController>();
            // TODO: Fix
            
            
        }

        // Update is called once per frame
        void Update()
        {
            FollowPlayer();
        }

        public void FollowPlayer()
        {
            if (_isPlayerControllerNotNull)
            {
                // To avoid multiple property access of built in components (should increase efficiency) 
                var position = _playerController.transform.position;
                var transform1 = transform;
                var bounds = cameraBounds.bounds;
            
                // Camera follow the player and bounds of camera.
                // if camera reached the end or the beginning of the map,_orthographicWidth and _orthographicHeight will stop camera from passing the boarder
                transform1.position = new Vector3(
                    Mathf.Clamp(position.x, bounds.min.x + _orthographicWidth, bounds.max.x - _orthographicWidth),
                    Mathf.Clamp(position.y, cameraBounds.bounds.min.y + _orthographicHeight, bounds.max.y - _orthographicHeight),
                    transform1.position.z);
            }
        }

        public void PlayLevelMusic(string sceneName)
        {
            switch (sceneName)
            {
                case "Level_1":
                    AudioManager.instance.PlayLevelOneMusic();
                    break;
                case "Level_2":
                    AudioManager.instance.PlayLevelTwoMusic();
                    break;
                case "Level_3":
                    AudioManager.instance.PlayLevelThreeMusic();
                    break;
                default:
                    AudioManager.instance.PlayLevelOneMusic();
                    break;
            }
        }
    }
}
