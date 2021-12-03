using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.GameManagement;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.UI;


namespace Assets.BackToSchool.Scripts.Models
{
    public class MainMenuModel : BaseModel
    {
        private IMainMenuPresenter _mainMenuPresenter;
        private IGameManager _gameManager;

        public MainMenuModel(IGameManager gameManager, IViewFactory viewFactory)
        {
            _gameManager                         =  gameManager;
            _mainMenuPresenter                   =  viewFactory.CreateView<IMainMenuPresenter, EViews>(EViews.MainMenu);
            _mainMenuPresenter.ExitTriggered     += ExitGame;
            _mainMenuPresenter.StartTriggered    += StartGame;
            _mainMenuPresenter.ContinueTriggered += ContinueGame;

            _mainMenuPresenter.ShowContinueButton(_gameManager.IsSaveDataExists());
        }

        private void ContinueGame() => _gameManager.StartGame(new StartParameters(false));

        private void ExitGame() => _gameManager.ExitGame();

        private void StartGame() => _gameManager.StartGame(new StartParameters(true));

        public override void Dispose()
        {
            _mainMenuPresenter.ExitTriggered     -= ExitGame;
            _mainMenuPresenter.StartTriggered    -= StartGame;
            _mainMenuPresenter.ContinueTriggered -= ContinueGame;
        }
    }
}