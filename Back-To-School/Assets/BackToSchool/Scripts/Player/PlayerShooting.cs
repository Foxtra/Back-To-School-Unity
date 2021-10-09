using System;
using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Weapon;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        public delegate void PlayerShootingHandler(PlayerShooting sender, PlayerAmmoArgs _args);

        public event PlayerShootingHandler OnAmmoChanged;

        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private GameObject _shootingPosition;
        [SerializeField] private float _reloadTime = 2.0f;
        [SerializeField] private float _bulletForce = 15.0f;
        [SerializeField] private int _maxAmmo = 10;

        private Bullet _bullet;
        private Animator _animator;
        private PlayerInteracting _playerInteracting;

        private int _currentAmmo;
        private bool _isReloading;

        private void Awake()
        {
            _currentAmmo = _maxAmmo;
            _animator = GetComponentInChildren<Animator>();
            _playerInteracting = GetComponent<PlayerInteracting>();
        }

        public void Fire()
        {
            if (!_isReloading && _currentAmmo != 0)
            {
                _bullet = Instantiate(_bulletPrefab);
                _bullet.transform.position = _shootingPosition.transform.position;
                _bullet.transform.rotation = _shootingPosition.transform.rotation;
                _bullet.Launch(_bulletForce);
                _currentAmmo--;
                if (OnAmmoChanged != null) OnAmmoChanged(this, new PlayerAmmoArgs(_currentAmmo, _maxAmmo));
            }
        }

        public void Reload()
        {
            _isReloading = true;
            _currentAmmo = _maxAmmo;
            _animator.SetTrigger(AnimationStates.Reload);
            Invoke(nameof(ReloadComplete), _reloadTime);
        }

        private void ReloadComplete()
        {
            _isReloading = false;
            if (OnAmmoChanged != null) OnAmmoChanged(this, new PlayerAmmoArgs(_currentAmmo, _maxAmmo));
        }
    }
}

public class PlayerAmmoArgs : EventArgs
{
    public int NewAmmoValue { get; }
    public int MaxAmmoValue { get; }

    public PlayerAmmoArgs(int newHealthValue, int maxAmmoValue)
    {
        NewAmmoValue = newHealthValue;
        MaxAmmoValue = maxAmmoValue;
    }
}