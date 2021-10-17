using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class HUDPresenter : MonoBehaviour
    {
        [SerializeField] private Text _ammoText;
        [SerializeField] private Slider _heathBar;

        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private float sliderValue;

        public void OnHealthChanged(float currentHealth, int maxHealth)
        {
            sliderValue = currentHealth / maxHealth;
            Invoke(nameof(UpdateHealthBar), _delayBeforeDamage);
        }

        public void OnAmmoChanged(int newAmmoValue, int maxAmmoValue) => UpdateAmmoText(newAmmoValue, maxAmmoValue);

        private void UpdateHealthBar() => _heathBar.value = sliderValue;

        private void UpdateAmmoText(int newAmmoValue, int maxAmmoValue) => _ammoText.text = $"{newAmmoValue} / {maxAmmoValue}";
    }
}