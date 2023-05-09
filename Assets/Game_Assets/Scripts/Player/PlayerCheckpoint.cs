using System;
using UnityEngine;

namespace Game_Assets.Scripts.Player
{
    public class PlayerCheckpoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                // Put this Vector3 position to be a new checkpoint
                PlayerRespawnController.instance.SetCheckpoint(transform.position);
            }
        }
    }
}
