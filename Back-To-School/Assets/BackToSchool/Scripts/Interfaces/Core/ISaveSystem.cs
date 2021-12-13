using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Player;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface ISaveSystem
    {
        public bool IsSaveDataExists();

        public void SavePlayerProgress(PlayerData playerData);

        public void SaveObjectiveProgress(ObjectiveParameters objectives);

        public void SaveLevelParameters(LevelParameters levelParameters);

        public PlayerData LoadPlayerProgress();

        public ObjectiveParameters LoadObjectiveProgress();

        public LevelParameters LoadLevelParameters();

        public void ResetAllSaveData();
    }
}