using Assets.BackToSchool.Scripts.Player;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class SaveSystem
    {
        public bool IsSaveDataExists() => PlayerPrefs.GetInt(Constants.SaveParams.IsSaveDataExists.ToString()) == 1;

        public void SavePlayerProgress(PlayerController player, LevelSystem levelSystem)
        {
            PlayerPrefs.SetInt(Constants.SaveParams.PlayerLevel.ToString(), levelSystem.GetLevelNumber());
            PlayerPrefs.SetInt(Constants.SaveParams.PlayerExperience.ToString(), levelSystem.GetExperience());
            PlayerPrefs.SetInt(Constants.SaveParams.PlayerAmmo.ToString(), player.WeaponController.GetAmmoValue());
            PlayerPrefs.SetInt(Constants.SaveParams.PlayerWeapon.ToString(), player.Inventory.GetCurrentWeaponNumber());
            PlayerPrefs.SetFloat(Constants.SaveParams.PlayerHealth.ToString(), player.GetHealthValue());

            PlayerPrefs.SetInt(Constants.SaveParams.IsSaveDataExists.ToString(), 1);
        }

        public void LoadPlayerProgress(PlayerController player, LevelSystem levelSystem)
        {
            player.WeaponController.SetAmmoValue(PlayerPrefs.GetInt(Constants.SaveParams.PlayerAmmo.ToString()));
            player.WeaponController.SetWeapon(PlayerPrefs.GetInt(Constants.SaveParams.PlayerWeapon.ToString()));
            player.SetHealthValue(PlayerPrefs.GetFloat(Constants.SaveParams.PlayerHealth.ToString()));
            levelSystem.SetLevelNumber(PlayerPrefs.GetInt(Constants.SaveParams.PlayerLevel.ToString()));
            levelSystem.AddExperience(PlayerPrefs.GetInt(Constants.SaveParams.PlayerExperience.ToString()));
        }

        public void ResetPlayerProgress() => PlayerPrefs.DeleteAll();

        public int GetPlayerLevel() => PlayerPrefs.GetInt(Constants.SaveParams.PlayerLevel.ToString());
    }
}