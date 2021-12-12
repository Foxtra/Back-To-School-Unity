using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class AssaultRifle : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _shootingPosition;

        private IBullet _bullet;
        private IResourceManager _resourceManager;
        private IAudioManager _audioManager;
        private float _bulletForce = Constants.WeaponStats.BulletForce;

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);

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

        public WeaponStats WeaponStats { get; private set; }
        public int CurrentAmmo { get; private set; }

        public void SetAmmo(int ammo) => CurrentAmmo = ammo;

        public void Attack(float damage)
        {
            _bullet = _resourceManager.CreateBullet(_shootingPosition);
            _bullet.SetDamage(damage);
            _bullet.Launch(_bulletForce);
            _audioManager.PlayEffect(ESounds.AssaultRifleShot);
        }

        public void FinishReload() { }
    }
}