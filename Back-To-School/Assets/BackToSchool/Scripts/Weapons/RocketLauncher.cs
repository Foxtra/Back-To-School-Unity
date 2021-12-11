using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class RocketLauncher : MonoBehaviour, IWeapon
    {
        // --- Audio ---
        private AudioSource _fireSource;
        private AudioSource _reloadSource;
        public AudioClip FireClip;
        public AudioClip ReloadClip;

        // --- Muzzle ---
        public GameObject muzzlePosition;

        public GameObject HideOnFire;

        private IBullet _rocket;
        private IResourceManager _resourceManager;
        public WeaponStats WeaponStats { get; private set; }
        public int CurrentAmmo { get; private set; }

        public void SetAmmo(int ammo) => CurrentAmmo = ammo;

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);

        /// <summary>
        ///     Creates an instance of the muzzle flash.
        ///     Also creates an instance of the audioSource so that multiple shots are not overlapped on the same audio source.
        ///     Insert projectile code in this function.
        /// </summary>
        public void Attack(float damage)
        {
            // --- Spawn muzzle flash ---
            var flash = _resourceManager.CreateMuzzleFlash(muzzlePosition.transform);

            _rocket = _resourceManager.CreateRocket(muzzlePosition.transform);
            _rocket.SetDamage(damage);
            HideOnFire.SetActive(false);

            // --- Handle Audio ---
            if (_fireSource != null)
            {
                // --- Sometimes the source is not attached to the weapon for easy instantiation on quick firing weapons like machineguns, 
                // so that each shot gets its own audio source, but sometimes it's fine to use just 1 source. We don't want to instantiate 
                // the parent gameobject or the program will get stuck in a loop, so we check to see if the source is a child object ---
                if (_fireSource.transform.IsChildOf(transform))
                    _fireSource.Play();
            }

            // --- Insert custom code here to shoot projectile or hitscan from weapon ---
        }

        public void FinishReload()
        {
            HideOnFire.SetActive(true);
            _reloadSource.Play();
        }

        public void Initialize(WeaponStats weaponStats, IResourceManager resourceManager, Transform weaponTransform,
            Transform parenTransform)
        {
            WeaponStats        = weaponStats;
            _resourceManager   = resourceManager;
            transform.position = weaponTransform.position;
            transform.rotation = weaponTransform.rotation;
            transform.parent   = parenTransform;
            SetAmmo(weaponStats.MaxAmmo.GetValue());
            Hide();
        }

        private void Start()
        {
            _fireSource          = new GameObject().AddComponent<AudioSource>();
            _reloadSource        = new GameObject().AddComponent<AudioSource>();
            _reloadSource.volume = 0.1f;
            _fireSource.clip     = FireClip;
            _reloadSource.clip   = ReloadClip;
        }
    }
}