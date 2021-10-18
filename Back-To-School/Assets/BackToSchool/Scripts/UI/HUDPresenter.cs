using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class HUDPresenter : MonoBehaviour
    {
        [SerializeField] private Text _ammoText;
        [SerializeField] private Text _levelText;
        [SerializeField] private Slider _heathBar;
        [SerializeField] private Slider _levelBar;

        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private float _healthSliderValue;
        private float _currentHealth;
        private int _maxHealth;
        private int _ammoValue;
        private int _maxAmmoValue;

        public void OnHealthChanged(float newCurrentHealth)
        {
            _currentHealth     = newCurrentHealth;
            _healthSliderValue = _currentHealth / _maxHealth;
            Invoke(nameof(UpdateHealthBar), _delayBeforeDamage);
        }

        public void OnMaxHealthChanged(int newMaxHealth)
        {
            _maxHealth         = newMaxHealth;
            _healthSliderValue = _currentHealth / _maxHealth;
            UpdateHealthBar();
        }

        public void OnAmmoChanged(int newAmmoValue)
        {
            _ammoValue = newAmmoValue;
            UpdateAmmoText();
        }

        public void OnMaxAmmoChanged(int newMaxAmmoValue)
        {
            _maxAmmoValue = newMaxAmmoValue;
            UpdateAmmoText();
        }

        public void OnLevelChanged(int newLevel) => UpdateLevelText(newLevel);

        public void OnExpChanged(float newSliderValue) { UpdateLevelBar(newSliderValue); }

        private void UpdateHealthBar()                      => _heathBar.value = _healthSliderValue;
        private void UpdateLevelBar(float levelSliderValue) => _levelBar.value = levelSliderValue;

        private void UpdateAmmoText()           => _ammoText.text = $"{_ammoValue} / {_maxAmmoValue}";
        private void UpdateLevelText(int level) => _levelText.text = $"Level: {level}";
    }
}