using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class MainMenuPresenter : MonoBehaviour
    {
        public event Action ExitTriggered;
        public event Action StartTriggered;
        public event Action ContinueTriggered;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _exitGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _continueGameButton.onClick.AddListener(ContinueGame);
            _exitGameButton.onClick.AddListener(ExitGame);
        }

        private void ExitGame() => ExitTriggered?.Invoke();

        private void StartGame() => StartTriggered?.Invoke();

        private void ContinueGame() => ContinueTriggered?.Invoke();
    }
}