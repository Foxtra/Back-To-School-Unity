using System;
using Assets.BackToSchool.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class MainMenuPresenter : MonoBehaviour
    {
        public event Action Exiting;
        public event Action Starting;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _exitGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _continueGameButton.onClick.AddListener(ContinueGame);
            _exitGameButton.onClick.AddListener(ExitGame);
        }

        private void ExitGame() { Exiting?.Invoke(); }

        private void StartGame()
        {
            GlobalSettings.IsNewGame = true;
            Starting?.Invoke();
        }

        private void ContinueGame()
        {
            GlobalSettings.IsNewGame = false;
            Starting?.Invoke();
        }
    }
}