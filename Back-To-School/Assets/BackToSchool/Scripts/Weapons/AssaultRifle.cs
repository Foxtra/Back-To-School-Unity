using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class AssaultRifle : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _shootingPosition;
        [SerializeField] private float _bulletForce;

        private IBullet _bullet;
        private IResourceManager _resourceManager;

        public void Hide() => gameObject.SetActive(false);
        public void Show() => gameObject.SetActive(true);

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

        public WeaponStats WeaponStats { get; private set; }
        public int CurrentAmmo { get; private set; }

        public void SetAmmo(int ammo) => CurrentAmmo = ammo;

        public void Attack(float damage)
        {
            _bullet = _resourceManager.CreateBullet(_shootingPosition);
            _bullet.SetDamage(damage);
            _bullet.Launch(_bulletForce);
        }

        public void FinishReload() { }
    }
}