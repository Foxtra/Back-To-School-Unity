using Assets.BackToSchool.Scripts.Enemies;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        private Slider _slider;
        private BaseEnemy _enemy;

        private void Awake() { _slider = GetComponent<Slider>(); }

        private void Start()
        {
            _enemy               =  GetComponentInParent<BaseEnemy>();
            _enemy.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float currentHealth, int maxHealth) => UpdateHealthBar(currentHealth / maxHealth);

        private void UpdateHealthBar(float newValue) => _slider.value = newValue;
    }
}