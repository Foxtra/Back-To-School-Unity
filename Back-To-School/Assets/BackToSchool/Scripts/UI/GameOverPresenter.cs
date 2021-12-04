using System;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class GameOverPresenter : MonoBehaviour, IGameOverPresenter
    {
        public event Action Restarted;

        [SerializeField] private Button _gameOverRestartButton;

        private void Awake() { _gameOverRestartButton.onClick.AddListener(Restart); }

        private void Restart() => Restarted?.Invoke();

        public void SetRoot(RectTransform canvas) => transform.SetParent(canvas, false);

        public void ShowView() => gameObject.SetActive(true);
        public void HideView() => gameObject.SetActive(false);
    }
}