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
            _enemy.OnHealthChanged += EnemyHealthBar_OnHealthChanged;
        }

        private void EnemyHealthBar_OnHealthChanged(Enemy sender, EnemyArgs _args)
        {
            UpdateHealthBar(_args.NewHealthValue);
        }

        private void UpdateHealthBar(float newValue)
        {
            _slider.value = newValue;
        }
    }
}