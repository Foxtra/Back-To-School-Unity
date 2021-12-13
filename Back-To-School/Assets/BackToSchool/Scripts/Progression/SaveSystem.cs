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

            PlayerPrefs.SetString(ESaveParams.ObjectivesData.ToStringCached(), json);
            PlayerPrefs.SetInt(ESaveParams.IsSaveDataExists.ToStringCached(), 1);
        }

        public void SaveLevelParameters(LevelParameters levelParameters)
        {
            var json = JsonConvert.SerializeObject(levelParameters);

            PlayerPrefs.SetString(ESaveParams.LevelParameters.ToStringCached(), json);
        }

        public PlayerData LoadPlayerProgress()
        {
            var json = PlayerPrefs.GetString(ESaveParams.PlayerData.ToStringCached());
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        public ObjectiveParameters LoadObjectiveProgress()
        {
            var json = PlayerPrefs.GetString(ESaveParams.ObjectivesData.ToStringCached());
            return JsonConvert.DeserializeObject<ObjectiveParameters>(json);
        }

        public LevelParameters LoadLevelParameters()
        {
            var json = PlayerPrefs.GetString(ESaveParams.LevelParameters.ToStringCached());
            return JsonConvert.DeserializeObject<LevelParameters>(json);
        }

        public void ResetAllSaveData() => PlayerPrefs.DeleteAll();
    }
}