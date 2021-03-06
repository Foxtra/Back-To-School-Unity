using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Player;
using Newtonsoft.Json;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class SaveSystem : ISaveSystem
    {
        public bool IsSaveDataExists() => PlayerPrefs.GetInt(ESaveParams.IsSaveDataExists.ToStringCached()) == 1;

        public void SavePlayerProgress(PlayerData playerData)
        {
            var json = JsonConvert.SerializeObject(playerData);

            PlayerPrefs.SetString(ESaveParams.PlayerData.ToStringCached(), json);
            PlayerPrefs.SetInt(ESaveParams.IsSaveDataExists.ToStringCached(), 1);
        }

        public void SaveObjectiveProgress(ObjectiveParameters objectives)
        {
            var json = JsonConvert.SerializeObject(objectives);

            PlayerPrefs.SetString(ESaveParams.ObjectivesData.ToString(), json);
            PlayerPrefs.SetInt(ESaveParams.IsSaveDataExists.ToString(), 1);
        }

        public PlayerData LoadPlayerProgress()
        {
            var json = PlayerPrefs.GetString(ESaveParams.PlayerData.ToStringCached());
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        public ObjectiveParameters LoadObjectiveProgress()
        {
            var json = PlayerPrefs.GetString(ESaveParams.ObjectivesData.ToString());
            return JsonConvert.DeserializeObject<ObjectiveParameters>(json);
        }

        public void ResetPlayerProgress() => PlayerPrefs.DeleteAll();
    }
}