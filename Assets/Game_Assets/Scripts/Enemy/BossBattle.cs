using Game_Assets.Scripts.Audio;
using Game_Assets.Scripts.Camera;
using Game_Assets.Scripts.Player;
using UnityEngine;

namespace Game_Assets.Scripts.Enemy
{
    public class BossBattle : MonoBehaviour
    {

        private CameraController _camera;
        public Transform cameraPosition;
        public float cameraSpeed;

        // When certain amount of health disappears, new phase will begin
        public int phaseOneMinHealth;
        public int phaseTwoMinHealth;
        
        public float projectilesIntervals;
        public float projectilesIntervalsPhaseThree;
        private float _projectileTimeCountdown; //time in-between shots
        public GameObject projectile; // prefab of projectile
        public Transform projectileStartPosition; // From where the projectile while start the shot.

        public Transform[] spawnPoints;
        private Transform _targetPoint;
        public float movementSpeed;
        public Animator animator;
        public Transform boss;

        public float activeTimeAmount; // How long will enemy be vulnerable 
        public float vanishTimeAmount; // How long does it take to vanish
        public float inactiveTimeAmount; // How long will it stay disappeared

        private float _activeTimeCountdown; 
        private float _vanishTimeCountdown;
        private float _inactiveCountdown;
        private static readonly int Vanish = Animator.StringToHash("Vanish");
        private bool _isTargetPointNull;

        public GameObject winObjects;
        private bool _isBattleOver;
        private bool _iswinObjectsNotNull;

        // Start is called before the first frame update
        void Start()
        {
            _iswinObjectsNotNull = winObjects != null;
            // Find and disable main camera
            _camera = FindObjectOfType<CameraController>();
            _camera.enabled = false;
            
            // Boss appear
            _activeTimeCountdown = activeTimeAmount;
            
            //Set projectile countdown time
            _projectileTimeCountdown = projectilesIntervals;
            
            AudioManager.instance.PlayBossFightMusic();

        }

        // Update is called once per frame
        void Update()
        {
            _camera.transform.position = 
                Vector3.MoveTowards(_camera.transform.position, cameraPosition.position, cameraSpeed * Time.deltaTime);
            if (!_isBattleOver)
            {
                      // First phase if current hp of boss is higher
                if (BossHealthController.instance.bossCurrentHp > phaseOneMinHealth)
                {
                    if (_activeTimeCountdown > 0)
                    {
                        _activeTimeCountdown -= Time.deltaTime;
                        // Trigger vanish animation and activate the timer.
                        if (_activeTimeCountdown <= 0)
                        {
                            _vanishTimeCountdown = vanishTimeAmount;
                            animator.SetTrigger(Vanish);
                        }

                        _projectileTimeCountdown -= Time.deltaTime;
                        if (_projectileTimeCountdown <= 0)
                        {
                            _projectileTimeCountdown = projectilesIntervals;
                            // Shoot projectile
                            Instantiate(projectile, projectileStartPosition.position, Quaternion.identity); // own rotation
                        }
                    }
                    else if (_vanishTimeCountdown > 0)
                    {
                        _vanishTimeCountdown -= Time.deltaTime;
                        if (_vanishTimeCountdown <= 0)
                        {
                            // Boss invisible
                            boss.gameObject.SetActive(false);
                            _inactiveCountdown = inactiveTimeAmount;
                        }
                    }
                    else if (_inactiveCountdown > 0)
                    {
                        _inactiveCountdown -= Time.deltaTime;
                        if (_inactiveCountdown <= 0)
                        {
                            // Respawn at random point (does not take max value)
                            boss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                            // Activate the object again
                            boss.gameObject.SetActive(true);
                            _activeTimeCountdown = activeTimeAmount; 
                            // Set time countdown to stop the shooting
                            _projectileTimeCountdown = projectilesIntervals;
                        }
                    }
                }
                else
                {
                    // In second phase we start to ignore active time and on appearance boss will move to spawn point and disappear, reappearing at the different location
                    // if boss at position
                    if (_targetPoint == null)
                    {
                        // Set target point as a boss position
                        _targetPoint = boss;
                        // reset vanish timer
                        _vanishTimeCountdown = vanishTimeAmount;
                        // start animation
                        animator.SetTrigger(Vanish);
                    }
                    else
                    {
                        // if boss is further from target than 0.3f move 
                        if (Vector3.Distance(boss.position, _targetPoint.position) > 0.3f)
                        {
                            // move from current boss location to the spawn point location
                            boss.position = Vector3.MoveTowards(boss.position, _targetPoint.position, movementSpeed * Time.deltaTime);
                            
                            if (Vector3.Distance(boss.position, _targetPoint.position) <= 0.3f)
                            {
                                _vanishTimeCountdown = vanishTimeAmount;
                                animator.SetTrigger(Vanish);
                            }
                            
                            
                            _projectileTimeCountdown -= Time.deltaTime;
                            if (_projectileTimeCountdown <= 0)
                            {
                                //decide between phase 2 and 3 projectile intervals
                                if (BossHealthController.instance.bossCurrentHp > phaseTwoMinHealth)
                                    _projectileTimeCountdown = projectilesIntervals;
                                
                                else
                                    _projectileTimeCountdown = projectilesIntervalsPhaseThree;
                                
                                // Shoot projectile
                                Instantiate(projectile, projectileStartPosition.position, Quaternion.identity); // own rotation
                            }
                        }
                        else if (_vanishTimeCountdown > 0)
                        {
                            _vanishTimeCountdown -= Time.deltaTime;
                            if (_vanishTimeCountdown <= 0)
                            {
                                boss.gameObject.SetActive(false);
                                _inactiveCountdown = inactiveTimeAmount;
                            }
                        }
                        else if (_inactiveCountdown > 0)
                        {
                            // Respawn at random point (does not take max value)
                            boss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                            // target to which boss will move when it appear
                            _targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            // To avoid picking the same position as the boss location
                            var whileInfinityBreak = 0;
                            while (_targetPoint.position == boss.position && whileInfinityBreak < 15)
                            {
                                _targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                                whileInfinityBreak++;
                            }
                            // Activate the object again
                            boss.gameObject.SetActive(true);
                            
                            //decide between phase 2 and 3 projectile intervals
                            if (BossHealthController.instance.bossCurrentHp > phaseTwoMinHealth)
                                _projectileTimeCountdown = projectilesIntervals;
                                
                            else
                                _projectileTimeCountdown = projectilesIntervalsPhaseThree;
                        }
                    }
                } 
            }
            else
            {
                _vanishTimeCountdown = vanishTimeAmount;
                
                    if (_iswinObjectsNotNull)
                    {
                        // Enable objects after win
                        winObjects.SetActive(true);
                        // Otherwise it will disappear along side with the boss
                        winObjects.transform.SetParent(null);
                    }

                    // enable main camera
                _camera.enabled = true;
                // Deactivate "Boss Battle" Object
                gameObject.SetActive(false);

            }
          
        }

        public void EndBattle()
        {
            _isBattleOver = true;
            _vanishTimeCountdown = vanishTimeAmount;
            // Play animation
            animator.SetTrigger(Vanish);
            // Disable collider on death 
            boss.GetComponent<Collider2D>().enabled = false;
            // Destroy all the boss projectiles.
            var projectiles = FindObjectsOfType<BossProjectile>();
            if (projectiles.Length > 0)
            {
                foreach (var bossProjectile in projectiles)
                {
                    Destroy(bossProjectile.gameObject);
                }
            }
        }
    }
}
