using System;
using Assets.BackToSchool.Scripts.Items;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IWeaponController
    {
        event Action<int> AmmoChanged;
        event Action<int> MaxAmmoChanged;
        event Action<int> WeaponChanged;
        event Action WeaponReloaded;

        void SetInventory(Inventory inventory);
        int  GetAmmoValue();
        int  GetWeaponIndex();
        void SetAmmoValue(int ammo);
        void SetWeapon(int weaponNumber);
        void NextWeapon(bool isNext);
        void Shoot(float playerDamage);
        void Reload();
        void ReloadComplete();
        void InitializeAmmo(int ammo);
        void InitializeWeapon(int weaponIndex);
        void UpdateHUD();
    }
}