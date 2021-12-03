using System;
using Assets.BackToSchool.Scripts.Stats;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IStatsManager
    {
        event Action<int> MaxHealthChanged;
        event Action<int> ArmorChanged;
        event Action<int> DamageChanged;
        event Action<int> MoveSpeedChanged;

        void Initialize(PlayerStats playerStats, int initialLevel);
        void LevelUp(int level);
    }
}