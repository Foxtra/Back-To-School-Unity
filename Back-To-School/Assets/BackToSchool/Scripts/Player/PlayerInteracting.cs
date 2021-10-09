using System;
using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerInteracting : MonoBehaviour
    {
        public event Action<int, int, int> HealthChanged;
        public event Action Death;

        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private float _damageTime = 0.1f;

        private SkinnedMeshRenderer[] _renderers;
        private Animator _animator;

        private int _currentHealth;
        private bool _isDead;

        public void GetDamage()
        {
            _currentHealth--;
            HealthChanged?.Invoke(_currentHealth, _maxHealth, 1);

            if (_currentHealth == 0 && !_isDead)
            {
                _animator.SetTrigger(AnimationStates.Die);
                _isDead = true;
                Death?.Invoke();
            }
            else if (_currentHealth > 0)
            {
                foreach (var renderer in _renderers)
                {
                    renderer.material.color = Color.red;
                }

                Invoke(nameof(ChangeColor), _damageTime);
            }
        }

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        private void ChangeColor()
        {
            foreach (var renderer in _renderers)
            {
                renderer.material.color = Color.white;
            }
        }
    }
}