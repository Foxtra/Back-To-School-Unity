using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class Presenter : MonoBehaviour
    {
        public event Action Restarted;
        public event Action Continued;

        //HUD
        [SerializeField] private Text _ammoText;
        [SerializeField] private Slider _heathBar;

        //Screens
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _PausePanel;
        [SerializeField] private Button _pauseRestartButton;
        [SerializeField] private Button _pauseContinueButton;
        [SerializeField] private Button _gameOverRestartButton;

        public void SwitchPausePanel(bool isPausePanelShowed) => _PausePanel.SetActive(isPausePanelShowed);

        public void ShowGameOverPanel() => _gameOverPanel.SetActive(true);

        public void OnHealthChanged(int currentHealth, int maxHealth, int damage) => UpdateHealthBar((float) currentHealth / maxHealth);

        public void OnAmmoChanged(int newAmmoValue, int maxAmmoValue) => UpdateAmmoText(newAmmoValue, maxAmmoValue);

        private void Awake()
        {
            _gameOverRestartButton.onClick.AddListener(Restart);
            _pauseRestartButton.onClick.AddListener(Restart);
            _pauseContinueButton.onClick.AddListener(Continue);
        }

        private void Continue() { Continued?.Invoke(); }

        private void Restart() { Restarted?.Invoke(); }

        private void UpdateHealthBar(float newValue) => _heathBar.value = newValue;

        private void UpdateAmmoText(int newAmmoValue, int maxAmmoValue) => _ammoText.text = $"{newAmmoValue} / {maxAmmoValue}";
    }
}