using System;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Items;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        public event Action<int> AmmoChanged;
        public event Action<int> WeaponChanged;
        public event Action WeaponReloaded;
        public event Action<int> MaxAmmoChanged;

        [SerializeField] private Transform _weaponPosition;

        private IWeapon _activeWeapon;
        private GameObject _activeWeaponObj;
        private GameObject _weaponToCreate;
        private Inventory _inventory;

        private bool _isReloading;

        public void SetInventory(Inventory inventory) => _inventory = inventory;

        public int GetAmmoValue() => _activeWeapon.CurrentAmmo;

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
            InitializeAmmo();
        }

        public void Shoot(float playerDamage)
        {
            if (!_isReloading && _activeWeapon.CurrentAmmo != 0)
            {
                _activeWeapon.CurrentAmmo--;
                AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
                _activeWeapon.Attack(playerDamage);
            }
        }

        public void Reload()
        {
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

        public void InitializeAmmo()
        {
            _activeWeapon.CurrentAmmo = _activeWeapon.WeaponStats.MaxAmmo.GetValue();
            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
            MaxAmmoChanged?.Invoke(_activeWeapon.WeaponStats.MaxAmmo.GetValue());
        }

        public void InitializeWeapon()
        {
            _weaponToCreate = _inventory.GetInitialWeapon();
            _activeWeapon   = ShowWeapon();
            InitializeAmmo();
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