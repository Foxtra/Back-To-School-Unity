using Assets.BackToSchool.Scripts.GameManagement;
using Cysharp.Threading.Tasks;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IGameManager
    {
        bool    IsSaveDataExists();
        void    ExitGame();
        void    StartGame(StartParameters parameters);
        void    ReturnToMenu();
        void    RestartLevel(string sceneName);
        void    InitializeMenu();
        void    InitializeMainScene();
        UniTask LoadMenu();
        UniTask LoadGame(StartParameters parameters);
    }
}