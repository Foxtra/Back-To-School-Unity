using System;
using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Stats;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class StatsManager : IStatsManager
    {
        public event Action<int> MaxHealthChanged;
        public event Action<int> ArmorChanged;
        public event Action<int> DamageChanged;
        public event Action<int> MoveSpeedChanged;

        private PlayerStats _playerStats;
        private Dictionary<EPlayerStats, int[]> _playerProgression = Constants.PlayerProgression;

        public void Initialize(PlayerStats playerStats, int initialLevel)
        {
            _playerStats = playerStats;
            for (var i = 0; i <= initialLevel; i++)
                LevelUp(i);
        }

        public void LevelUp(int level)
        {
            _playerStats.Armor.AddModifier(_playerProgression[EPlayerStats.Armor][level]);
            _playerStats.Damage.AddModifier(_playerProgression[EPlayerStats.Damage][level]);
            _playerStats.MaxHealth.AddModifier(_playerProgression[EPlayerStats.MaxHealth][level]);
            _playerStats.MoveSpeed.AddModifier(_playerProgression[EPlayerStats.MoveSpeed][level]);

            UpdateHud();
        }

        private void UpdateHud()
        {
            ArmorChanged?.Invoke(_playerStats.Armor.GetValue());
            DamageChanged?.Invoke(_playerStats.Damage.GetValue());
            MaxHealthChanged?.Invoke(_playerStats.MaxHealth.GetValue());
            MoveSpeedChanged?.Invoke(_playerStats.MoveSpeed.GetValue());
        }
    }
}