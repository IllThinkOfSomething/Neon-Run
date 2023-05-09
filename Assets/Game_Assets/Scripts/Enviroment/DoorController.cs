using System.Collections;
using Game_Assets.Scripts.Player;
using Game_Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Assets.Scripts.Enviroment
{
    public class DoorController : MonoBehaviour
    {
        private bool _playerExitingLevel;
        private PlayerController _playerController;

        public Transform exitPosition;

        public float playerMovementSpeed;

        public string nextLevelToLoad;
        // Start is called before the first frame update
        void Start()
        {
            // Access player controller
            _playerController = PlayerHealthController.instance.GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            
            if (_playerExitingLevel)
            {
                // if player is transitioning the level the game will take control and make the player move to the required location
                _playerController.transform.position =
                    Vector3.MoveTowards(_playerController.transform.position, exitPosition.position, playerMovementSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (!_playerExitingLevel)
                {
                    // Freeze player
                    _playerController.ableToMove = false;
                    
                    StartCoroutine(UseDoorCoroutine());
                }
            }
        }

        IEnumerator UseDoorCoroutine()
        {
            _playerExitingLevel = true;

            // Stop animation and slide to the next level starting point location
            _playerController.animator.enabled = false;
            // Screen starts to fade black 
            UIController.instance.StartFading();
            
            // Wait
            yield return new WaitForSeconds(1.5f);
            // Set new checkpoint
            PlayerRespawnController.instance.SetCheckpoint(exitPosition.position);
            //Enable animation
            _playerController.animator.enabled = true;
            // Enable movement
            _playerController.ableToMove = true;
            
            // Start to rise transparency 
            UIController.instance.StartFadingFromBlack();
            // Load next scene
            SceneManager.LoadScene(nextLevelToLoad);
        }
    }
}
