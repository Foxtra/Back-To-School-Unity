using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class AssaultRifle : MonoBehaviour, IWeapon
    {
        [SerializeField] private Transform _shootingPosition;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _bulletForce;

        private Bullet _bullet;

        public AssaultRifle() => WeaponStats = new WeaponStats(1, 10, 1);

        public WeaponStats WeaponStats { get; set; }
        public int CurrentAmmo { get; set; }

        public void Attack(float damage)
        {
            _bullet                    = Instantiate(_bulletPrefab);
            _bullet.transform.position = _shootingPosition.position;
            _bullet.transform.rotation = _shootingPosition.rotation;
            _bullet.transform.parent   = null;
            _bullet.SetDamage(damage);
            _bullet.Launch(_bulletForce);
        }

        public void ReloadFinished() { }
    }
}