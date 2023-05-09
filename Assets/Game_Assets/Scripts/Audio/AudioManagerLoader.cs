using UnityEngine;

namespace Game_Assets.Scripts.Audio
{
    public class AudioManagerLoader : MonoBehaviour
    {
        public AudioManager audioManager;
        private void Awake()
        {
            Invoke("", 5);
            // Check if audio manager instance exists
            if (AudioManager.instance == null)
            {
                // if it does not exist create assign it to singleton and do not destroy on scene loading
                AudioManager newAudioManager = Instantiate(audioManager);
                AudioManager.instance = newAudioManager;
                DontDestroyOnLoad(newAudioManager.gameObject);

            }
        }
    }
}
