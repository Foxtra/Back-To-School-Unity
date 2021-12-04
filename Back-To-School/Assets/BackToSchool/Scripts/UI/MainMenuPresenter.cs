using System;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class MainMenuPresenter : MonoBehaviour, IMainMenuPresenter
    {
        public event Action ExitTriggered;
        public event Action StartTriggered;
        public event Action ContinueTriggered;

        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _exitGameButton;

        public void ShowContinueButton(bool isShown) => _continueGameButton.gameObject.SetActive(isShown);
        public void SetRoot(RectTransform canvas)    => transform.SetParent(canvas, false);
        public void ShowView()                       => gameObject.SetActive(true);
        public void HideView()                       => gameObject.SetActive(false);

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _continueGameButton.onClick.AddListener(ContinueGame);
            _exitGameButton.onClick.AddListener(ExitGame);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            _continueGameButton.onClick.RemoveListener(ContinueGame);
            _exitGameButton.onClick.RemoveListener(ExitGame);
        }

        private void ExitGame() => ExitTriggered?.Invoke();

        private void StartGame() => StartTriggered?.Invoke();

        private void ContinueGame() => ContinueTriggered?.Invoke();
    }
}