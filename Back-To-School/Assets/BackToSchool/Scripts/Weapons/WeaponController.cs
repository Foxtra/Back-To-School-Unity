using System;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Items;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class WeaponController : MonoBehaviour, IWeaponController
    {
        public event Action<int> AmmoChanged;
        public event Action<int> MaxAmmoChanged;
        public event Action<int> WeaponChanged;
        public event Action WeaponReloaded;

        [SerializeField] private Transform _weaponPosition;

        private IWeapon _activeWeapon;
        private GameObject _activeWeaponObj;
        private GameObject _weaponToCreate;
        private Inventory _inventory;

        private bool _isReloading;

        public void SetInventory(Inventory inventory) => _inventory = inventory;

        public void UpdateHUD()
        {
            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
            MaxAmmoChanged?.Invoke(_activeWeapon.WeaponStats.MaxAmmo.GetValue());
            WeaponChanged?.Invoke(_inventory.GetCurrentWeaponNumber());
        }

        public int GetAmmoValue()   => _activeWeapon.CurrentAmmo;
        public int GetWeaponIndex() => _inventory.GetCurrentWeaponNumber();

        public void SetAmmoValue(int ammo)
        {
            _activeWeapon.CurrentAmmo = ammo;
            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
            MaxAmmoChanged?.Invoke(_activeWeapon.WeaponStats.MaxAmmo.GetValue());
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
            InitializeAmmo(0);
        }

        public void Shoot(float playerDamage)
        {
            if (!_isReloading && _activeWeapon.CurrentAmmo != 0)
            {
                _activeWeapon.CurrentAmmo--;
                AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
                _activeWeapon.Attack(playerDamage);
            }
            else Reload();
        }

        public void Reload()
        {
            if (_isReloading)
                return;

            _isReloading              = true;
            _activeWeapon.CurrentAmmo = _activeWeapon.WeaponStats.MaxAmmo.GetValue();
            WeaponReloaded?.Invoke();
        }

        public void ReloadComplete()
        {
            _isReloading = false;
            _activeWeapon.ReloadFinished();
            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
        }

        public void InitializeAmmo(int ammo)
        {
            _activeWeapon.CurrentAmmo = ammo == 0 ? _activeWeapon.WeaponStats.MaxAmmo.GetValue() : ammo;

            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
            MaxAmmoChanged?.Invoke(_activeWeapon.WeaponStats.MaxAmmo.GetValue());
        }

        public void InitializeWeapon(int weaponIndex)
        {
            _weaponToCreate = weaponIndex == 0 ? _inventory.GetInitialWeapon() : _inventory.GetWeapon(weaponIndex);
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