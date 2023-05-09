using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Assets.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public string newGameScene;
        
        // Start is called before the first frame update
        void Start()
        {
            AudioManager.instance.mainMenuMusic.Play();
        }

        public void NewGame()
        {
            AudioManager.instance.AdjustSfxPitch(11);
            SceneManager.LoadScene(newGameScene);
            Time.timeScale = 1f;
        }

        public void QuitApplication()
        {
            AudioManager.instance.AdjustSfxPitch(11);
            Application.Quit();
            Debug.Log("Quit");
            
        }

      
    }
}
