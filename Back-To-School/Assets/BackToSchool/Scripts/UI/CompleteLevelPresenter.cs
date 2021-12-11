using System;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class CompleteLevelPresenter : View, ICompleteLevelPresenter
    {
        public event Action Restarted;
        public event Action MenuReturned;

        [SerializeField] private Button _levelRestartButton;
        [SerializeField] private Button _returnToMenuButton;

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