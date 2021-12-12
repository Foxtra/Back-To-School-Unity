using System;
using Assets.BackToSchool.Scripts.Stats;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IStatsManager
    {
        public event Action<int> MaxHealthChanged;
        public event Action<int> ArmorChanged;
        public event Action<int> DamageChanged;
        public event Action<int> MoveSpeedChanged;

        public void Initialize(PlayerStats playerStats, int initialLevel);
        public void LevelUp(int level);
    }
}