using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class RocketLauncher : MonoBehaviour, IWeapon
    {
        public GameObject muzzlePosition;
        public GameObject HideOnFire;

        private IBullet _rocket;
        private IResourceManager _resourceManager;
        private IAudioManager _audioManager;
        public WeaponStats WeaponStats { get; private set; }
        public int CurrentAmmo { get; private set; }

        public void SetAmmo(int ammo) => CurrentAmmo = ammo;

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);

        public void Attack(float damage)
        {
            var flash = _resourceManager.CreateMuzzleFlash(muzzlePosition.transform);

            _rocket = _resourceManager.CreateRocket(muzzlePosition.transform);
            _rocket.SetDamage(damage);
            _rocket.Launch(Constants.WeaponStats.RocketForce);
            HideOnFire.SetActive(false);
            _audioManager.PlayEffect(ESounds.RocketLauncherFire);
        }

        public void FinishReload()
        {
            HideOnFire.SetActive(true);
            _audioManager.PlayEffect(ESounds.RocketLauncherReload);
        }

        public void Initialize(WeaponStats weaponStats, IResourceManager resourceManager, IAudioManager audioManager,
            Transform weaponTransform, Transform parenTransform)
        {
            WeaponStats        = weaponStats;
            _resourceManager   = resourceManager;
            _audioManager      = audioManager;
            transform.position = weaponTransform.position;
            transform.rotation = weaponTransform.rotation;
            transform.parent   = parenTransform;
            SetAmmo(weaponStats.MaxAmmo.GetValue());
            Hide();
        }
    }
}