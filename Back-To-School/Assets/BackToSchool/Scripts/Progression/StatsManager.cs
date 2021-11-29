using System;
using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Interfaces;
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
        private Dictionary<string, int[]> _playerProgression = new Dictionary<string, int[]>
        {
            ["Armor"]     = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            ["Damage"]    = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            ["MaxHealth"] = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10 },
            ["MoveSpeed"] = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        public void Initialize(PlayerStats playerStats, int initialLevel)
        {
            _playerStats = playerStats;
            for (var i = 0; i <= initialLevel; i++)
                LevelUp(i);
        }

        public void LevelUp(int level)
        {
            _playerStats.Armor.AddModifier(_playerProgression["Armor"][level]);
            _playerStats.Damage.AddModifier(_playerProgression["Damage"][level]);
            _playerStats.MaxHealth.AddModifier(_playerProgression["MaxHealth"][level]);
            _playerStats.MoveSpeed.AddModifier(_playerProgression["MoveSpeed"][level]);

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