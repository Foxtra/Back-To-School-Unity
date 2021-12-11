using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IWeapon
    {
        public WeaponStats WeaponStats { get; }
        public int CurrentAmmo { get; }
        public void SetAmmo(int ammo);
        public void Attack(float damage);
        public void ReloadFinished();

        public void Show();
        public void Hide();

        public void Initialize(WeaponStats weaponStats, IResourceManager resourceManager, Transform weaponTransform,
            Transform parenTransform);
    }
}