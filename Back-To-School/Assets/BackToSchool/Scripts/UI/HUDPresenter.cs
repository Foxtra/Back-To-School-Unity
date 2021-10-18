using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class HUDPresenter : MonoBehaviour
    {
        [SerializeField] private Text _ammoText;
        [SerializeField] private Slider _heathBar;

        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private float _sliderValue;
        private float _currentHealth;
        private int _maxHealth;
        private int _ammoValue;
        private int _maxAmmoValue;

        public void OnHealthChanged(float newCurrentHealth)
        {
            _currentHealth = newCurrentHealth;
            _sliderValue   = _currentHealth / _maxHealth;
            Invoke(nameof(UpdateHealthBar), _delayBeforeDamage);
        }

        public void OnMaxHealthChanged(int newMaxHealth)
        {
            _maxHealth   = newMaxHealth;
            _sliderValue = _currentHealth / _maxHealth;
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

        private void UpdateHealthBar() => _heathBar.value = _sliderValue;

        private void UpdateAmmoText() => _ammoText.text = $"{_ammoValue} / {_maxAmmoValue}";
    }
}