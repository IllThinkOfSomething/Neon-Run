using System;
using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public class BossBattleActivator : MonoBehaviour
    {
        public GameObject activateBattle;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Enable object when trigger is activated
            if (col.CompareTag("Player"))
            {
                activateBattle.SetActive(true);
            
                // Disable activator object.
                gameObject.SetActive(false);
            }

            
        }
    }
}
