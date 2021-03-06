using System;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class PausePresenter : View, IPausePresenter
    {
        public event Action Restarted;
        public event Action Continued;
        public event Action MenuReturned;

        [SerializeField] private Button _pauseRestartButton;
        [SerializeField] private Button _pauseContinueButton;
        [SerializeField] private Button _returnToMenuButton;

        public void TogglePausePanel(bool isPausePanelShowed) => gameObject.SetActive(isPausePanelShowed);

        private void Awake()
        {
            _pauseRestartButton.onClick.AddListener(Restart);
            _pauseContinueButton.onClick.AddListener(Continue);
            _returnToMenuButton.onClick.AddListener(Return);
        }

        private void OnDestroy()
        {
            _pauseRestartButton.onClick.RemoveListener(Restart);
            _pauseContinueButton.onClick.RemoveListener(Continue);
            _returnToMenuButton.onClick.RemoveListener(Return);
        }

        private void Continue() => Continued?.Invoke();

        private void Restart() => Restarted?.Invoke();
        private void Return()  => MenuReturned?.Invoke();
    }
}