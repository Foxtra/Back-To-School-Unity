using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.GameManagement;
using Assets.BackToSchool.Scripts.UI;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuPresenter _mainMenuPresenter;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager                                =  GameManager.Instance;
            _mainMenuPresenter.ExitTriggered            += ExitGame;
            _mainMenuPresenter.KillEnemiesModeTriggered += StartGame;
            _mainMenuPresenter.SurviveModeTriggered     += StartGame;
            _mainMenuPresenter.ContinueTriggered        += ContinueGame;

            _mainMenuPresenter.ShowContinueButton(_gameManager.IsSaveDataExists());
        }

        private void ContinueGame() => _gameManager.StartGame(new StartParameters(false));

        private void ExitGame() => _gameManager.ExitGame();

        private void StartGame(GameModes mode) => _gameManager.StartGame(new StartParameters(true, mode));

        private void OnDestroy()
        {
            _mainMenuPresenter.ExitTriggered            -= ExitGame;
            _mainMenuPresenter.KillEnemiesModeTriggered -= StartGame;
            _mainMenuPresenter.SurviveModeTriggered     -= StartGame;
            _mainMenuPresenter.ContinueTriggered        -= ContinueGame;
        }
    }
}