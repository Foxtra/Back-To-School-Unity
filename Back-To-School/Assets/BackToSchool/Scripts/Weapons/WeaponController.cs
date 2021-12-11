using System;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Items;
using Cysharp.Threading.Tasks;
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
        private IResourceManager _resourceManager;
        private WeaponList _weaponList;

        private bool _isReloading;

        public void Initialize(WeaponList weaponList, IResourceManager resourceManager, int ammo, int weaponIndex)
        {
            _weaponList      = weaponList;
            _resourceManager = resourceManager;

            var allWeapons = _weaponList.GetAllWeaponTypes();
            _weaponList.SetWeapons(_resourceManager.CreateAllWeapons(allWeapons, _weaponPosition, gameObject.transform));

            _activeWeapon = _weaponList.Weapons[weaponIndex];
            _activeWeapon.Show();

            _activeWeapon.SetAmmo(ammo == 0 ? _activeWeapon.WeaponStats.MaxAmmo.GetValue() : ammo);
            WeaponChanged?.Invoke(weaponIndex);
        }

        public int GetAmmoValue()    => _activeWeapon.CurrentAmmo;
        public int GetMaxAmmoValue() => _activeWeapon.WeaponStats.MaxAmmo.GetValue();
        public int GetWeaponIndex()  => _weaponList.GetCurrentWeaponNumber();

        public void SetAmmoValue(int ammo)
        {
            _activeWeapon.SetAmmo(ammo);
            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
            MaxAmmoChanged?.Invoke(_activeWeapon.WeaponStats.MaxAmmo.GetValue());
        }

        public void SetWeapon(int weaponNumber)
        {
            if (_activeWeapon != null)
            {
                _activeWeapon.Hide();
                _activeWeapon = null;
            }

            _activeWeapon = _weaponList.GetWeapon(weaponNumber);
            _activeWeapon.Show();
        }

        public void NextWeapon(bool isNext)
        {
            if (_activeWeapon != null)
            {
                _activeWeapon.Hide();
                _activeWeapon = null;
            }

            _activeWeapon = isNext ? _weaponList.GetNextWeapon() : _weaponList.GetPreviousWeapon();
            _activeWeapon.Show();

            WeaponChanged?.Invoke(_weaponList.GetCurrentWeaponNumber());
            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
            MaxAmmoChanged?.Invoke(_activeWeapon.WeaponStats.MaxAmmo.GetValue());
        }

        public void Shoot(float playerDamage)
        {
            if (!_isReloading && _activeWeapon.CurrentAmmo != 0)
            {
                _activeWeapon.SetAmmo(_activeWeapon.CurrentAmmo - 1);
                AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
                _activeWeapon.Attack(playerDamage);
            }
            else StartReloading();
        }

        public void StartReloading()
        {
            if (_isReloading)
                return;

            _isReloading = true;
            _activeWeapon.SetAmmo(_activeWeapon.WeaponStats.MaxAmmo.GetValue());
            WeaponReloaded?.Invoke();
        }

        public async void FinishReloading(int reloadingTime)
        {
            await UniTask.Delay(reloadingTime);

            _isReloading = false;
            _activeWeapon.FinishReload();
            AmmoChanged?.Invoke(_activeWeapon.CurrentAmmo);
        }
    }
}