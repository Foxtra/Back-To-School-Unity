using System;
using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Weapon;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        public event Action<int, int> AmmoChanged;

        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private GameObject _shootingPosition;
        [SerializeField] private float _reloadTime = 2.0f;
        [SerializeField] private float _bulletForce = 15.0f;
        [SerializeField] private int _maxAmmo = 10;
        [SerializeField] private int _playerDamage = 1;

        private Bullet _bullet;
        private Animator _animator;

        private int _currentAmmo;
        private bool _isReloading;

        public void Fire()
        {
            if (!_isReloading && _currentAmmo != 0)
            {
                _bullet                    = Instantiate(_bulletPrefab);
                _bullet.transform.position = _shootingPosition.transform.position;
                _bullet.transform.rotation = _shootingPosition.transform.rotation;
                _bullet.SetDamage(_playerDamage);
                _bullet.Launch(_bulletForce);
                _currentAmmo--;
                AmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
            }
        }

        public void Reload()
        {
            _isReloading = true;
            _currentAmmo = _maxAmmo;
            _animator.SetTrigger(AnimationStates.Reload);
            Invoke(nameof(ReloadComplete), _reloadTime);
        }

        private void Awake()
        {
            _currentAmmo = _maxAmmo;
            _animator    = GetComponentInChildren<Animator>();
        }

        private void ReloadComplete()
        {
            _isReloading = false;
            AmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
        }
    }
}