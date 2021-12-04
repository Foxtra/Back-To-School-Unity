using Assets.BackToSchool.Scripts.Player;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface ISaveSystem
    {
        bool       IsSaveDataExists();
        void       SavePlayerProgress(PlayerData playerData);
        PlayerData LoadPlayerProgress();
        void       ResetPlayerProgress();
    }
}