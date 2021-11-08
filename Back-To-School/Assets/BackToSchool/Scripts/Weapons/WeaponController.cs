﻿using System;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Items;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        public event Action<int> AmmoChanged;
        public event Action<int> WeaponChanged;
        public event Action WeaponReloaded;

        [SerializeField] private Transform _weaponPosition;

        private IWeapon _activeWeapon;
        private GameObject _activeWeaponObj;
        private GameObject _weaponToCreate;
        private Inventory _inventory;
        private PlayerStats _playerStats;

        private int _currentAmmo;
        private bool _isReloading;

        public void SetInventory(Inventory inventory) => _inventory = inventory;

        public void SetPlayerStats(PlayerStats playerStats) => _playerStats = playerStats;
        // TODO move max ammo to weapon

        public int GetAmmoValue() => _currentAmmo;

        public void SetAmmoValue(int ammo)
        {
            _currentAmmo = ammo;
            AmmoChanged?.Invoke(_currentAmmo);
        }

        public void SetWeapon(int weaponNumber)
        {
            if (_activeWeapon != null) HideWeapon();
            _weaponToCreate = _inventory.GetWeapon(weaponNumber);
            _activeWeapon   = ShowWeapon();
        }

        public void NextWeapon(bool isNext)
        {
            if (_activeWeapon != null) HideWeapon();
            _weaponToCreate = isNext ? _inventory.GetNextWeapon() : _inventory.GetPreviousWeapon();
            _activeWeapon   = ShowWeapon();
        }

        public void Shoot(float playerDamage)
        {
            if (!_isReloading && _currentAmmo != 0)
            {
                _currentAmmo--;
                AmmoChanged?.Invoke(_currentAmmo);
                _activeWeapon.Attack(playerDamage);
            }
        }

        public void Reload()
        {
            _isReloading = true;
            _currentAmmo = _playerStats.MaxAmmo.GetValue();
            WeaponReloaded?.Invoke();
        }

        public void ReloadComplete()
        {
            _isReloading = false;
            AmmoChanged?.Invoke(_currentAmmo);
        }

        public void InitializeAmmo()
        {
            _currentAmmo = _playerStats.MaxAmmo.GetValue();
            AmmoChanged?.Invoke(_currentAmmo);
        }

        public void InitializeWeapon()
        {
            _weaponToCreate = _inventory.GetInitialWeapon();
            _activeWeapon   = ShowWeapon();
        }

        private IWeapon ShowWeapon()
        {
            _activeWeaponObj                  = Instantiate(_weaponToCreate, _weaponPosition.position, _weaponPosition.rotation);
            _activeWeaponObj.transform.parent = gameObject.transform;
            WeaponChanged?.Invoke(_inventory.GetCurrentWeaponNumber());
            return _activeWeaponObj.GetComponent<IWeapon>();
        }

        private void HideWeapon()
        {
            Destroy(_activeWeaponObj);
            _activeWeapon = null;
        }
    }
}