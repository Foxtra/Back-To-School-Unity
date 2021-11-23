using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Player;
using Newtonsoft.Json;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class SaveSystem
    {
        public bool IsSaveDataExists() => PlayerPrefs.GetInt(SaveParams.IsSaveDataExists.ToString()) == 1;

        public void SavePlayerProgress(PlayerData playerData)
        {
            var json = JsonConvert.SerializeObject(playerData);

            PlayerPrefs.SetString(SaveParams.PlayerData.ToString(), json);
            PlayerPrefs.SetInt(SaveParams.IsSaveDataExists.ToString(), 1);
        }

        public void SaveObjectiveProgress(ObjectiveParameters objectives)
        {
            var json = JsonConvert.SerializeObject(objectives);

            PlayerPrefs.SetString(SaveParams.ObjectivesData.ToString(), json);
            PlayerPrefs.SetInt(SaveParams.IsSaveDataExists.ToString(), 1);
        }

        public PlayerData LoadPlayerProgress()
        {
            var json = PlayerPrefs.GetString(SaveParams.PlayerData.ToString());
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        public ObjectiveParameters LoadObjectiveProgress()
        {
            var json = PlayerPrefs.GetString(SaveParams.ObjectivesData.ToString());
            return JsonConvert.DeserializeObject<ObjectiveParameters>(json);
        }

        public void ResetPlayerProgress() => PlayerPrefs.DeleteAll();
    }
}