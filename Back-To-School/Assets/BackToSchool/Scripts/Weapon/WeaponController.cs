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
        private Inventory _inventory;

        public float ReloadTime { get; private set; }

        public void SetInventory(Inventory inventory) => _inventory = inventory;

        public void ChangeWeapon(bool isNext)
        {
            if (_activeWeapon != null) HideWeapon();
            _activeWeaponObj = isNext ? _inventory.GetNextWeapon() : _inventory.GetPreviousWeapon();
            ShowWeapon();
        }

        public void Shoot(float playerDamage) => _activeWeapon.Attack(playerDamage);

        private void Start()
        {
            ReloadTime = 2f;
            GetInitialWeapon();
        }

        private void GetInitialWeapon()
        {
            _activeWeaponObj = _inventory.GetInitialWeapon();
            _activeWeapon    = ShowWeapon();
        }

        private IWeapon ShowWeapon()
        {
            var weapon = Instantiate(_activeWeaponObj, _weaponPosition.position, _weaponPosition.rotation);
            weapon.transform.parent = gameObject.transform;
            return weapon.GetComponent<IWeapon>();
        }

        private void HideWeapon()
        {
            Destroy(_activeWeaponObj);
            _activeWeapon = null;
        }
    }
}