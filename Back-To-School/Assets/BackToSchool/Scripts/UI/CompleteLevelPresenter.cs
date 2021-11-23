using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class CompleteLevelPresenter : MonoBehaviour
    {
        public event Action Restarted;
        public event Action MenuReturned;

        [SerializeField] private Button _levelRestartButton;
        [SerializeField] private Button _returnToMenuButton;

        public void ShowCompleteLevelPanel() => gameObject.SetActive(true);

        private void Awake()
        {
            _levelRestartButton.onClick.AddListener(Restart);
            _returnToMenuButton.onClick.AddListener(Return);
        }

        private void OnDestroy()
        {
            _levelRestartButton.onClick.RemoveListener(Restart);
            _returnToMenuButton.onClick.RemoveListener(Return);
        }

        private void Restart() => Restarted?.Invoke();
        private void Return()  => MenuReturned?.Invoke();
    }
}