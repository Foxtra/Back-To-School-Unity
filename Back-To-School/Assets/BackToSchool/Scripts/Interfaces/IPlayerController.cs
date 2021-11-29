using System;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Stats;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IPlayerController : IMovable, IShootable, IDamageable
    {
        event Action<float> HealthChanged;
        event Action Died;
        event Action<int> AmmoChanged;
        event Action<int> WeaponChanged;
        event Action<int> MaxAmmoChanged;

        void  Initialize(IPlayerInput playerInput, PlayerStats playerStats, PlayerData playerData);
        void  Reload();
        void  ReloadFinished();
        void  NextWeapon(bool isNext);
        float GetHealthValue();
        int   GetAmmoValue();
        int   GetActiveWeaponIndex();
        void  SetHealthValue(float health);
    }
}