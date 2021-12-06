using System;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class CompleteLevelPresenter : MonoBehaviour, ICompleteLevelPresenter
    {
        public event Action Restarted;
        public event Action MenuReturned;

        [SerializeField] private Button _levelRestartButton;
        [SerializeField] private Button _returnToMenuButton;

        public void SetRoot(RectTransform canvas) => transform.SetParent(canvas, false);

        public void ShowView() => gameObject.SetActive(true);
        public void HideView() => gameObject.SetActive(false);

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