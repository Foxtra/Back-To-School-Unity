using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Player;
using Newtonsoft.Json;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class SaveSystem : ISaveSystem
    {
        public bool IsSaveDataExists() => PlayerPrefs.GetInt(SaveParams.IsSaveDataExists.ToString()) == 1;

        public void SavePlayerProgress(PlayerData playerData)
        {
            var json = JsonConvert.SerializeObject(playerData);

            PlayerPrefs.SetString(SaveParams.PlayerData.ToString(), json);
            PlayerPrefs.SetInt(SaveParams.IsSaveDataExists.ToString(), 1);
        }

        public PlayerData LoadPlayerProgress()
        {
            var json = PlayerPrefs.GetString(SaveParams.PlayerData.ToString());
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        public void ResetPlayerProgress() => PlayerPrefs.DeleteAll();
    }
}