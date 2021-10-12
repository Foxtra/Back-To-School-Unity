using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class MainMenuPresenter : MonoBehaviour
    {
        public event Action Exiting;
        public event Action Starting;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _exitGameButton.onClick.AddListener(ExitGame);
        }

        private void ExitGame() { Exiting?.Invoke(); }

        private void StartGame() { Starting?.Invoke(); }
    }
}