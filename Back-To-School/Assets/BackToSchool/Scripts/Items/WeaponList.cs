using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Items
{
    public class WeaponList : MonoBehaviour
    {
        public List<IWeapon> Weapons { get; private set; }
        private List<EWeapons> _weaponTypes = new List<EWeapons> { EWeapons.AssaultRifle, EWeapons.RocketLauncher };

        private int _currentWeaponNumber;

        public void SetWeapons(List<IWeapon> weapons) => Weapons = weapons;

        public List<EWeapons> GetAllWeaponTypes() => _weaponTypes;

        public int GetCurrentWeaponNumber() => _currentWeaponNumber;

        public IWeapon GetInitialWeapon() => Weapons[0];

        public IWeapon GetWeapon(int weaponNumber)
        {
            _currentWeaponNumber = weaponNumber;
            return Weapons[weaponNumber];
        }

        public IWeapon GetNextWeapon()
        {
            _currentWeaponNumber++;
            if (_currentWeaponNumber < Weapons.Count)
                return Weapons[_currentWeaponNumber];

            _currentWeaponNumber = 0;
            return Weapons[_currentWeaponNumber];
        }

        public IWeapon GetPreviousWeapon()
        {
            _currentWeaponNumber--;
            if (_currentWeaponNumber >= 0)
                return Weapons[_currentWeaponNumber];

            _currentWeaponNumber = Weapons.Count - 1;
            return Weapons[_currentWeaponNumber];
        }
    }
}