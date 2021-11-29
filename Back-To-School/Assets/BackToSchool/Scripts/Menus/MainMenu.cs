using Assets.BackToSchool.Scripts.GameManagement;
using Assets.BackToSchool.Scripts.UI;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        private MainMenuPresenter _mainMenuPresenter;
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager                         =  GameManager.Instance;
            _mainMenuPresenter                   =  FindObjectOfType<MainMenuPresenter>(); //TODO UIRoot or ResourceManager?
            _mainMenuPresenter.ExitTriggered     += ExitGame;
            _mainMenuPresenter.StartTriggered    += StartGame;
            _mainMenuPresenter.ContinueTriggered += ContinueGame;

            _mainMenuPresenter.ShowContinueButton(_gameManager.IsSaveDataExists());
        }

        private void ContinueGame() => _gameManager.StartGame(new StartParameters(false));

        private void ExitGame() => _gameManager.ExitGame();

        private void StartGame() => _gameManager.StartGame(new StartParameters(true));

        private void OnDestroy()
        {
            _mainMenuPresenter.ExitTriggered     -= ExitGame;
            _mainMenuPresenter.StartTriggered    -= StartGame;
            _mainMenuPresenter.ContinueTriggered -= ContinueGame;
        }
    }
}