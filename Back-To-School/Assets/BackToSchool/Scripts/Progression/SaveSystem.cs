using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    internal class SaveSystem
    {
        private Player.Player _player;
        private StatsManager _statsManager;

        public SaveSystem(Player.Player player, StatsManager statsManager)
        {
            _player       = player;
            _statsManager = statsManager;
        }

        public bool IsSaveDataExists() => PlayerPrefs.GetInt(SaveParams.IsSaveDataExists) == 1;

        public void OnPlayerProgressChanged()
        {
            PlayerPrefs.SetInt(SaveParams.PlayerLevel, _player.LevelSystem.GetLevelNumber());
            PlayerPrefs.SetInt(SaveParams.PlayerExperience, _player.LevelSystem.GetExperience());
            PlayerPrefs.SetInt(SaveParams.PlayerAmmo, _player.WeaponController.GetAmmoValue());
            PlayerPrefs.SetInt(SaveParams.PlayerWeapon, _player.Inventory.GetCurrentWeaponNumber());
            PlayerPrefs.SetFloat(SaveParams.PlayerHealth, _player.GetHealthValue());

            PlayerPrefs.SetInt(SaveParams.IsSaveDataExists, 1);
        }

        public void LoadPlayerProgress()
        {
            for (var i = 1; i <= PlayerPrefs.GetInt(SaveParams.PlayerLevel); i++) { _statsManager.OnLevelUp(i); }

            _player.WeaponController.SetAmmoValue(PlayerPrefs.GetInt(SaveParams.PlayerAmmo));
            _player.WeaponController.SetWeapon(PlayerPrefs.GetInt(SaveParams.PlayerWeapon));
            _player.SetHealthValue(PlayerPrefs.GetFloat(SaveParams.PlayerHealth));
            _player.LevelSystem.SetLevelNumber(PlayerPrefs.GetInt(SaveParams.PlayerLevel));
            _player.LevelSystem.AddExperience(PlayerPrefs.GetInt(SaveParams.PlayerExperience));
        }

        public void ResetPlayerProgress() => PlayerPrefs.DeleteAll();
    }
}