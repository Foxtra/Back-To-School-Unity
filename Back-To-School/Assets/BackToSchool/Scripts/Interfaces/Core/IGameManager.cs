using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Parameters;
using Cysharp.Threading.Tasks;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface IGameManager
    {
        public bool IsSaveDataExists();

        public void ExitGame();

        public UniTask StartGame(StartParameters parameters);

        public UniTask ReturnToMenu();

        public UniTask RestartLevel(string sceneName, EGameModes gameMode);

        public UniTask LoadMenu();

        public UniTask LoadGame(StartParameters parameters);
    }
}