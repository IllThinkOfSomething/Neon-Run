using System.Collections;
using System.Collections.Generic;
using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.Bomb;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float timeTillExplodes;
    public GameObject explosion;

    public float explosionRadius;
    public LayerMask DestructableLayers;
    
    // Update is called once per frame
    void Update()
    {
        Blowup();
    }

    public void Blowup()
    {
        timeTillExplodes -= Time.deltaTime;
        
        if (timeTillExplodes <= 0)
        {
            if (explosion != null)
            {
                var transform1 = transform;
                Instantiate(explosion, transform1.position, transform1.rotation);
            }
            AudioManager.instance.AdjustSfxPitch(5);
            Destroy(gameObject);

            // destroy objects that are in the explosion radius
            var objectsToDamage = Physics2D.OverlapCircleAll(transform.position, explosionRadius, DestructableLayers);

            if (objectsToDamage.Length > 0)
            {
                foreach (var destructible in objectsToDamage)
                {
                    Destroy(destructible.gameObject);
                }
            }
        }
    }

}
