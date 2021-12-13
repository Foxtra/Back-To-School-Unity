using Assets.BackToSchool.Scripts.Parameters;
using Cysharp.Threading.Tasks;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface IGameManager
    {
        public bool IsSaveDataExists();

        public void ExitGame();

        public UniTask StartGame(StartParameters parameters);

        public UniTask ContinueGame();

        public UniTask ReturnToMenu();

        public UniTask RestartLevel(LevelParameters parameters);

        public bool IsLastLevel(LevelParameters parameters);

        public UniTask LoadMenu();

        public UniTask LoadScene(StartParameters parameters);
    }
}