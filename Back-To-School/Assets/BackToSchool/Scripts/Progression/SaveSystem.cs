using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Player;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class SaveSystem
    {
        public bool IsSaveDataExists() => PlayerPrefs.GetInt(SaveParams.IsSaveDataExists.ToString()) == 1;

        public void SavePlayerProgress(PlayerController player, LevelSystem levelSystem)
        {
            PlayerPrefs.SetInt(SaveParams.PlayerLevel.ToString(), levelSystem.GetLevelNumber());
            PlayerPrefs.SetInt(SaveParams.PlayerExperience.ToString(), levelSystem.GetExperience());
            PlayerPrefs.SetInt(SaveParams.PlayerAmmo.ToString(), player.WeaponController.GetAmmoValue());
            PlayerPrefs.SetInt(SaveParams.PlayerWeapon.ToString(), player.Inventory.GetCurrentWeaponNumber());
            PlayerPrefs.SetFloat(SaveParams.PlayerHealth.ToString(), player.GetHealthValue());

            PlayerPrefs.SetInt(SaveParams.IsSaveDataExists.ToString(), 1);
        }

        public void LoadPlayerProgress(PlayerController player, LevelSystem levelSystem)
        {
            player.WeaponController.SetWeapon(PlayerPrefs.GetInt(SaveParams.PlayerWeapon.ToString()));
            player.WeaponController.SetAmmoValue(PlayerPrefs.GetInt(SaveParams.PlayerAmmo.ToString()));
            player.SetHealthValue(PlayerPrefs.GetFloat(SaveParams.PlayerHealth.ToString()));
            levelSystem.SetLevelNumber(PlayerPrefs.GetInt(SaveParams.PlayerLevel.ToString()));
            levelSystem.AddExperience(PlayerPrefs.GetInt(SaveParams.PlayerExperience.ToString()));
        }

        public void ResetPlayerProgress() => PlayerPrefs.DeleteAll();

        public int GetPlayerLevel() => PlayerPrefs.GetInt(SaveParams.PlayerLevel.ToString());
    }
}