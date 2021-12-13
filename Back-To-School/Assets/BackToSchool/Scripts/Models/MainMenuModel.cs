using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Parameters;


namespace Assets.BackToSchool.Scripts.Models
{
    public class MainMenuModel : Model
    {
        private IMainMenuPresenter _mainMenuPresenter;
        private IGameManager _gameManager;

        public MainMenuModel(IGameManager gameManager, IViewFactory viewFactory)
        {
            _gameManager                                =  gameManager;
            _mainMenuPresenter                          =  viewFactory.CreateView<IMainMenuPresenter, EViews>(EViews.MainMenu);
            _mainMenuPresenter.ExitTriggered            += ExitGame;
            _mainMenuPresenter.KillEnemiesModeTriggered += StartGame;
            _mainMenuPresenter.SurviveModeTriggered     += StartGame;
            _mainMenuPresenter.ContinueTriggered        += ContinueGame;
            _mainMenuPresenter.ShowContinueButton(_gameManager.IsSaveDataExists());
        }

        private void ContinueGame() => _gameManager.ContinueGame();

        private void ExitGame() => _gameManager.ExitGame();

        private void StartGame(EGameModes mode) => _gameManager.StartGame(new StartParameters(true, mode));

        public override void Dispose()
        {
            _mainMenuPresenter.ExitTriggered            -= ExitGame;
            _mainMenuPresenter.KillEnemiesModeTriggered -= StartGame;
            _mainMenuPresenter.SurviveModeTriggered     -= StartGame;
            _mainMenuPresenter.ContinueTriggered        -= ContinueGame;
        }
    }
}