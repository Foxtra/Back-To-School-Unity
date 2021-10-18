using System;
using System.Collections.Generic;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class StatsManager
    {
        public event Action<int> MaxAmmoChanged;
        public event Action<int> MaxHealthChanged;

        private Player.Player _player;
        private Dictionary<string, int[]> _playerProgression = new Dictionary<string, int[]>
        {
            ["Armor"]       = new[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 },
            ["Damage"]      = new[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 },
            ["FireRate"]    = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            ["MaxAmmo"]     = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10 },
            ["MaxHealth"]   = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10 },
            ["MoveSpeed"]   = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 10 },
            ["ReloadSpeed"] = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        public StatsManager(Player.Player player) => _player = player;

        public void OnLevelUp(int level)
        {
            _player.PlayerStats.Armor.AddModifier(_playerProgression["Armor"][level]);
            _player.PlayerStats.Damage.AddModifier(_playerProgression["Damage"][level]);
            //_player.PlayerStats.FireRate.AddModifier(_playerProgression["FireRate"][level]); TODO
            _player.PlayerStats.MaxAmmo.AddModifier(_playerProgression["MaxAmmo"][level]);
            _player.PlayerStats.MaxHealth.AddModifier(_playerProgression["MaxHealth"][level]);
            _player.PlayerStats.MoveSpeed.AddModifier(_playerProgression["MoveSpeed"][level]);
            //_player.PlayerStats.ReloadSpeed.AddModifier(_playerProgression["ReloadSpeed"][level]); TODO

            UpdateHUD();
        }

        private void UpdateHUD()
        {
            MaxAmmoChanged?.Invoke(_player.PlayerStats.MaxAmmo.GetValue());
            MaxHealthChanged?.Invoke(_player.PlayerStats.MaxHealth.GetValue());
        }
    }
}