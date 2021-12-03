using System;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Game
{
    public interface IPlayerController : IMovable, IShootable, IDamageable
    {
        event Action<float> HealthChanged;
        event Action Died;
        event Action<int> AmmoChanged;
        event Action<int> WeaponChanged;
        event Action<int> MaxAmmoChanged;

        GameObject gameObject { get; }

        void  Reload();
        void  ReloadFinished();
        void  NextWeapon(bool isNext);
        float GetHealthValue();
        int   GetAmmoValue();
        int   GetActiveWeaponIndex();
        void  SetHealthValue(float health);
    }
}