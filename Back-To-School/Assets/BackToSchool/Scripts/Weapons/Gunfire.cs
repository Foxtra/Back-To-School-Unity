using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class Gunfire : MonoBehaviour, IWeapon
    {
        // --- Audio ---
        public AudioClip GunShotClip;
        public AudioClip ReloadClip;
        public AudioSource source;
        public AudioSource reloadSource;
        public Vector2 audioPitch = new Vector2(.9f, 1.1f);

        // --- Muzzle ---
        public GameObject muzzlePrefab;
        public GameObject muzzlePosition;

        // --- Config ---
        public bool autoFire;
        public float shotDelay = .5f;
        public bool rotate = true;
        public float rotationSpeed = .25f;

        // --- Options ---
        public GameObject scope;
        public bool scopeActive = true;
        private bool lastScopeState;

        // --- Projectile ---
        [Tooltip("The projectile gameobject to instantiate each time the weapon is fired.")]
        public Rocket projectilePrefab;
        [Tooltip(
            "Sometimes a mesh will want to be disabled on fire. For example: when a rocket is fired, we instantiate a new rocket, and disable" +
            " the visible rocket attached to the rocket launcher")]
        public GameObject projectileToDisableOnFire;
        private Rocket _rocket;

        // --- Timing ---
        [SerializeField] private float timeLastFired;

        public Gunfire() => WeaponStats = new WeaponStats();
        public WeaponStats WeaponStats { get; set; }
        public int CurrentAmmo { get; set; }

        /// <summary>
        ///     Creates an instance of the muzzle flash.
        ///     Also creates an instance of the audioSource so that multiple shots are not overlapped on the same audio source.
        ///     Insert projectile code in this function.
        /// </summary>
        public void Attack(float damage)
        {
            // --- Keep track of when the weapon is being fired ---
            timeLastFired = Time.time;

            // --- Spawn muzzle flash ---
            var flash = Instantiate(muzzlePrefab, muzzlePosition.transform);


            // --- Shoot Projectile Object ---
            if (projectilePrefab != null)
            {
                _rocket = Instantiate(projectilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation,
                    transform);
                _rocket.SetDamage(damage);
                _rocket.transform.parent = null;
            }

            // --- Disable any gameobjects, if needed ---
            if (projectileToDisableOnFire != null) projectileToDisableOnFire.SetActive(false);

            // --- Handle Audio ---
            if (source != null)
            {
                // --- Sometimes the source is not attached to the weapon for easy instantiation on quick firing weapons like machineguns, 
                // so that each shot gets its own audio source, but sometimes it's fine to use just 1 source. We don't want to instantiate 
                // the parent gameobject or the program will get stuck in a loop, so we check to see if the source is a child object ---
                if (source.transform.IsChildOf(transform))
                    source.Play();
                else
                {
                    // --- Instantiate prefab for audio, delete after a few seconds ---
                    var newAS = Instantiate(source);
                    if ((newAS = Instantiate(source)) != null && newAS.outputAudioMixerGroup != null &&
                        newAS.outputAudioMixerGroup.audioMixer != null)
                    {
                        // --- Change pitch to give variation to repeated shots ---
                        newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", Random.Range(audioPitch.x, audioPitch.y));
                        newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);

                        // --- Play the gunshot sound ---
                        newAS.PlayOneShot(GunShotClip);

                        // --- Remove after a few seconds. Test script only. When using in project I recommend using an object pool ---
                        Destroy(newAS.gameObject, 4);
                    }
                }
            }

            // --- Insert custom code here to shoot projectile or hitscan from weapon ---
        }

        public void ReloadFinished() => ReEnableDisabledProjectile();

        private void Start()
        {
            if (source != null) source.clip = GunShotClip;
            timeLastFired  = 0;
            lastScopeState = scopeActive;
        }

        private void ReEnableDisabledProjectile()
        {
            reloadSource.Play();
            projectileToDisableOnFire.SetActive(true);
        }
    }
}