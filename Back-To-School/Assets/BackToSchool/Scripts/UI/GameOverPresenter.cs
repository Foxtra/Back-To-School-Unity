using System;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class GameOverPresenter : View, IGameOverPresenter
    {
        public event Action Restarted;

        [SerializeField] private Button _gameOverRestartButton;

        private void Awake() { _gameOverRestartButton.onClick.AddListener(Restart); }

        private void Restart() => Restarted?.Invoke();
    }
}