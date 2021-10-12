using System;
using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public event Action<int, int> HealthChanged;
        public event Action<Enemy> Died;

        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _stopDistance = 1f;
        [SerializeField] private float _attackInterval = 2f;
        [SerializeField] private int _maxHealth = 2;
        [SerializeField] private int _enemyDamage = 1;

        private GameObject _target;
        private Animator _animator;

        private float _damageTime = 1.2f;
        private float _deathTime = 1.5f;
        private float _timer;
        private int _currentHealth;

        private bool _isBusy;
        private bool _isDead;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            _isBusy = true;

            if (_currentHealth == 0 && !_isDead)
            {
                _animator.SetTrigger(AnimationStates.Die);
                _isDead = true;
            }
            else if (_currentHealth > 0) _animator.SetTrigger(AnimationStates.GetDamage);

            if (_currentHealth == 0 && _isDead) { EnemyDeath(); }
            else { Invoke(nameof(EnableEnemy), _damageTime); }
        }

        public void SetTarget(GameObject target) => _target = target;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _animator      = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;
            if (!_isBusy && _target)
            {
                transform.LookAt(_target.transform.position);

                if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position, _stopDistance))
                {
                    _animator.SetBool(AnimationStates.IsMoving, true);
                    transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.fixedDeltaTime);
                }
                else
                {
                    if (_timer > _attackInterval)
                    {
                        _isBusy = true;
                        Attack();
                        _timer = 0f;
                        Invoke(nameof(EnableEnemy), _attackInterval);
                    }
                }
            }
            else { _animator.SetBool(AnimationStates.IsMoving, false); }
        }

        private void Attack()
        {
            _animator.SetTrigger(AnimationStates.Attack);
            _target.GetComponent<IDamageable>().TakeDamage(_enemyDamage);
        }

        private void EnableEnemy()
        {
            if (!_isDead) _isBusy = false;
        }

        private void EnemyDeath()
        {
            Destroy(gameObject, _deathTime);
            Died?.Invoke(this);
        }
    }
}