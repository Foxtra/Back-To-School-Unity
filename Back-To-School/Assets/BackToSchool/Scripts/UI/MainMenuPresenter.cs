using System;
using Assets.BackToSchool.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class MainMenuPresenter : MonoBehaviour
    {
        public event Action ExitTriggered;
        public event Action ContinueTriggered;
        public event Action<GameModes> KillEnemiesModeTriggered;
        public event Action<GameModes> SurviveModeTriggered;

        [SerializeField] private GameObject _modeScreenSelection;

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
            gameObject.SetActive(false);
            _modeScreenSelection.SetActive(true);
        }

        private void ContinueGame()           => ContinueTriggered?.Invoke();
        private void StartGameInKillMode()    => KillEnemiesModeTriggered?.Invoke(GameModes.KillEnemies);
        private void StartGameInSurviveMode() => SurviveModeTriggered?.Invoke(GameModes.SurviveTime);

        private void BackToMainMenu()
        {
            _modeScreenSelection.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}