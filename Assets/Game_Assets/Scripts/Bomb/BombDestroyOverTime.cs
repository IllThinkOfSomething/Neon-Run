using UnityEngine;

namespace Game_Assets.Scripts.Bomb
{
    public class BombDestroyOverTime : MonoBehaviour
    {
        public float bombDuration;
        
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, bombDuration);
        }
    }
}