using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Items;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform _weaponPosition;

        private IWeapon _activeWeapon;
        private GameObject _activeWeaponObj;
        private GameObject _weaponToCreate;
        private Inventory _inventory;

        public float ReloadTime { get; private set; }

        public void SetInventory(Inventory inventory) => _inventory = inventory;

        public void NextWeapon(bool isNext)
        {
            if (_activeWeapon != null) HideWeapon();
            _weaponToCreate = isNext ? _inventory.GetNextWeapon() : _inventory.GetPreviousWeapon();
            _activeWeapon   = ShowWeapon();
        }

        public void Shoot(float playerDamage) => _activeWeapon.Attack(playerDamage);

        private void Start()
        {
            ReloadTime = 2f;
            GetInitialWeapon();
        }

        private void GetInitialWeapon()
        {
            _weaponToCreate = _inventory.GetInitialWeapon();
            _activeWeapon   = ShowWeapon();
        }

        private IWeapon ShowWeapon()
        {
            _activeWeaponObj                  = Instantiate(_weaponToCreate, _weaponPosition.position, _weaponPosition.rotation);
            _activeWeaponObj.transform.parent = gameObject.transform;
            return _activeWeaponObj.GetComponent<IWeapon>();
        }

        private void HideWeapon()
        {
            Destroy(_activeWeaponObj);
            _activeWeapon = null;
        }
    }
}