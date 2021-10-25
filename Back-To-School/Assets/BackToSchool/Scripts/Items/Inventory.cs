using System.Collections.Generic;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Items
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _weapons;

        private int _currentWeaponNumber;

        public GameObject GetInitialWeapon() => _weapons.Count != 0 ? _weapons[0] : null;

        public GameObject GetNextWeapon()
        {
            _currentWeaponNumber++;
            if (_currentWeaponNumber < _weapons.Count)
                return _weapons[_currentWeaponNumber];

            _currentWeaponNumber = 0;
            return _weapons[_currentWeaponNumber];
        }

        public GameObject GetPreviousWeapon()
        {
            _currentWeaponNumber--;
            if (_currentWeaponNumber < 0)
            {
                _currentWeaponNumber = _weapons.Count - 1;
                return _weapons[_currentWeaponNumber];
            }

            return _weapons[_currentWeaponNumber];
        }
    }
}