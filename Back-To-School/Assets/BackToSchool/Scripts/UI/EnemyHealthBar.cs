using Assets.BackToSchool.Scripts.Enemies;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        private Slider _slider;
        private Enemy _enemy;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            _enemy = GetComponentInParent<Enemy>();
            _enemy.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int currentHealth, int maxHealth, int damage)
        {
            UpdateHealthBar((float) currentHealth / maxHealth);
        }

        private void UpdateHealthBar(float newValue)
        {
            _slider.value = newValue;
        }
    }
}