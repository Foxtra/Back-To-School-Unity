using System;
using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Stats;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class StatsManager
    {
        public event Action<int> MaxAmmoChanged;
        public event Action<int> MaxHealthChanged;
        public event Action<int> ArmorChanged;
        public event Action<int> DamageChanged;
        public event Action<int> MoveSpeedChanged;

        private readonly PlayerStats _playerStats;
        private Dictionary<string, int[]> _playerProgression = new Dictionary<string, int[]>
        {
            ["Armor"]       = new[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            ["Damage"]      = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            ["FireRate"]    = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            ["MaxAmmo"]     = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10 },
            ["MaxHealth"]   = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10 },
            ["MoveSpeed"]   = new[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            ["ReloadSpeed"] = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        public StatsManager(PlayerStats playerStats, int initialLevel)
        {
            _playerStats = playerStats;
            for (var i = 1; i <= initialLevel; i++) { LevelUp(i); }
        }

        public void LevelUp(int level)
        {
            _playerStats.Armor.AddModifier(_playerProgression["Armor"][level]);
            _playerStats.Damage.AddModifier(_playerProgression["Damage"][level]);
            //_playerStats.FireRate.AddModifier(_playerProgression["FireRate"][level]); TODO
            _playerStats.MaxAmmo.AddModifier(_playerProgression["MaxAmmo"][level]);
            _playerStats.MaxHealth.AddModifier(_playerProgression["MaxHealth"][level]);
            _playerStats.MoveSpeed.AddModifier(_playerProgression["MoveSpeed"][level]);
            //_playerStats.ReloadSpeed.AddModifier(_playerProgression["ReloadSpeed"][level]); TODO

            UpdateHUD();
        }

        private void UpdateHUD()
        {
            ArmorChanged?.Invoke(_playerStats.Armor.GetValue());
            DamageChanged?.Invoke(_playerStats.Damage.GetValue());
            MaxAmmoChanged?.Invoke(_playerStats.MaxAmmo.GetValue());
            MaxHealthChanged?.Invoke(_playerStats.MaxHealth.GetValue());
            MoveSpeedChanged?.Invoke(_playerStats.MoveSpeed.GetValue());
        }
    }
}