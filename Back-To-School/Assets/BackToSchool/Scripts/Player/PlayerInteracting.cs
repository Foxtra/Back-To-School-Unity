using System;
using System.Collections;
using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.WeaponEffects;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerInteracting : MonoBehaviour, IDamageable
    {
        public event Action<int, int> HealthChanged;
        public event Action Died;

        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private float _damageTime = 0.1f;
        [SerializeField] private float _delayBeforeDamage = 0.5f;

        private SkinnedMeshRenderer[] _renderers;
        private Animator _animator;
        private WeaponType _weaponType;

        private int _currentHealth;
        private bool _isDead;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth == 0 && !_isDead)
            {
                _animator.SetTrigger(AnimationStates.Die);
                _isDead = true;
                Died?.Invoke();
            }
            else if (_currentHealth > 0)
                StartCoroutine(nameof(ShowDamageEffect));
        }

        private void Awake()
        {
            _animator   = GetComponentInChildren<Animator>();
            _renderers  = GetComponentsInChildren<SkinnedMeshRenderer>();
            _weaponType = GetComponent<WeaponType>();
        }

        private void Start()
        {
            _currentHealth =  _maxHealth;
            Died           += _weaponType.OnPlayerDeath;
        }

        private IEnumerator ShowDamageEffect()
        {
            yield return new WaitForSeconds(_delayBeforeDamage);
            ChangeColor(Color.red);
            yield return new WaitForSeconds(_damageTime);
            ChangeColor(Color.white);
        }

        private void ChangeColor(Color color)
        {
            foreach (var renderer in _renderers)
            {
                renderer.material.color = color;
            }
        }
    }
}