using Assets.BackToSchool.Scripts.Parameters;
using Cysharp.Threading.Tasks;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface IGameManager
    {
        bool    IsSaveDataExists();
        void    ExitGame();
        void    StartGame(StartParameters parameters);
        void    ReturnToMenu();
        void    RestartLevel(string sceneName);
        UniTask LoadMenu();
        UniTask LoadGame(StartParameters parameters);
    }
}