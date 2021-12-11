using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class MainMenuPresenter : View, IMainMenuPresenter
    {
        public event Action ExitTriggered;
        public event Action ContinueTriggered;
        public event Action<EGameModes> KillEnemiesModeTriggered;
        public event Action<EGameModes> SurviveModeTriggered;

        [SerializeField] private GameObject _mainView;
        [SerializeField] private GameObject _modeSelectionView;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _killEnemiesModeButton;
        [SerializeField] private Button _surviveModeButton;
        [SerializeField] private Button _backToMenuButton;

        public void ShowContinueButton(bool isShown) => _continueGameButton.gameObject.SetActive(isShown);

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _continueGameButton.onClick.AddListener(ContinueGame);
            _exitGameButton.onClick.AddListener(ExitGame);

            _killEnemiesModeButton.onClick.AddListener(StartGameInKillMode);
            _surviveModeButton.onClick.AddListener(StartGameInSurviveMode);
            _backToMenuButton.onClick.AddListener(BackToMainMenu);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            _continueGameButton.onClick.RemoveListener(ContinueGame);
            _exitGameButton.onClick.RemoveListener(ExitGame);
        }

        private void ExitGame() => ExitTriggered?.Invoke();

        private void StartGame()
        {
            _mainView.SetActive(false);
            _modeSelectionView.SetActive(true);
        }

        private void ContinueGame()           => ContinueTriggered?.Invoke();
        private void StartGameInKillMode()    => KillEnemiesModeTriggered?.Invoke(EGameModes.KillEnemies);
        private void StartGameInSurviveMode() => SurviveModeTriggered?.Invoke(EGameModes.SurviveTime);

        private void BackToMainMenu()
        {
            _modeSelectionView.SetActive(false);
            _mainView.SetActive(true);
        }
    }
}