using System;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Game
{
    public interface IPlayerController : IMovable, IShootable, IDamageable
    {
        public event Action<float> HealthChanged;
        public event Action Died;
        public event Action<int> AmmoChanged;
        public event Action<int> WeaponChanged;
        public event Action<int> MaxAmmoChanged;

        public Transform Transform { get; }

        public void  Reload();
        public void  NextWeapon(bool isNext);
        public float GetHealthValue();
        public int   GetAmmoValue();
        public int   GetActiveWeaponIndex();
        public void  SetHealthValue(float health);
    }
}