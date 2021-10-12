using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class GameOverPresenter : MonoBehaviour
    {
        public event Action Restarted;

        [SerializeField] private Button _gameOverRestartButton;

        public void ShowGameOverPanel() => gameObject.SetActive(true);

        private void Awake() { _gameOverRestartButton.onClick.AddListener(Restart); }

        private void Restart() => Restarted?.Invoke();
    }
}