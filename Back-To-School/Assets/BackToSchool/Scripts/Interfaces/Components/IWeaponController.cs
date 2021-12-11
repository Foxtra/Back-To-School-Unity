using System;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Items;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IWeaponController
    {
        public event Action<int> AmmoChanged;
        public event Action<int> MaxAmmoChanged;
        public event Action<int> WeaponChanged;
        public event Action WeaponReloaded;

        public void Initialize(WeaponList weaponList, IResourceManager resourceManager, int ammo, int weaponIndex);
        public int  GetAmmoValue();
        public int  GetMaxAmmoValue();
        public int  GetWeaponIndex();
        public void SetAmmoValue(int ammo);
        public void SetWeapon(int weaponNumber);
        public void NextWeapon(bool isNext);
        public void Shoot(float playerDamage);
        public void Reload();
        public void ReloadComplete();
    }
}