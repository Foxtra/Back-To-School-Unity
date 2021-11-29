using System.Collections;
using Assets.BackToSchool.Scripts.GameManagement;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IGameManager
    {
        bool        IsSaveDataExists();
        void        ExitGame();
        void        StartGame(StartParameters parameters);
        void        ReturnToMenu();
        void        RestartLevel(string sceneName);
        void        InitializeMenu();
        void        InitializeMainScene();
        IEnumerator LoadMenu();
        IEnumerator LoadGame(StartParameters parameters);
    }
}