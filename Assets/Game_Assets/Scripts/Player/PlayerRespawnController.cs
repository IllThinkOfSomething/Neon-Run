using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Assets.Scripts.Player
{
    public class PlayerRespawnController : MonoBehaviour
    {
        // Create singleton since it will be the only respawn controller in game.
        public static PlayerRespawnController instance;

        private Vector3 _respawnLocation;
        public float timeTillRespawned;
        
        private GameObject _player;
        public GameObject defeatEffect;
        private bool _isDefeatEffectNotNull;

        private void Awake()
        {
            // To save player's progress between scenes or on death
            if (instance == null)
            {
                instance = this;
                // On reload or next scene do not destroy the instance
                DontDestroyOnLoad(instance);
            }
            else
            {
                // Destroy game object if instance already exist, this avoid multiple player respawns
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _isDefeatEffectNotNull = defeatEffect != null;
            _player = PlayerHealthController.instance.gameObject;
            
            // Respawn position is the player starting position
            _respawnLocation = _player.transform.position;
        }

        public void SetCheckpoint(Vector3 position)
        {
            _respawnLocation = position;
        }
        
        public void Reload()
        {
            StartCoroutine(RespawnCoroutine());
        }
        IEnumerator RespawnCoroutine()
        {
            _player.SetActive(false);
            if(_isDefeatEffectNotNull)
            {
                var transform1 = transform;
                Instantiate(defeatEffect, _player.transform.position, _player.transform.rotation);
            }

            // Coroutine fails if it is set to 0
            if (timeTillRespawned <= 0)
                timeTillRespawned = 3;
            
            // Create another timeline(thread) and wait certain amount of time when activated;
            yield return new WaitForSeconds(timeTillRespawned);
            
            // Restart the scene when player dies to respawn all the elements of it. 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            _player.transform.position = _respawnLocation;
            _player.SetActive(true);
            
            PlayerHealthController.instance.ResetHealth();
        }
    }
}
