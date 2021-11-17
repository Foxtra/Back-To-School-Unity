using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public event Action<float, int> HealthChanged;
        public event Action<Enemy> Died;

        public CharacterStats EnemyStats;

        [SerializeField] private float _speed;
        [SerializeField] private float _stopDistance = 1f;
        [SerializeField] private float _attackInterval = 2f;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _enemyDamage;

        private GameObject _target;
        private Animator _animator;

        private float _damageTime = 1.2f;
        private float _deathTime = 1.5f;
        private float _timer;
        private float _currentHealth;

        private bool _isBusy;
        private bool _isDead;

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            _isBusy = true;

            if (_currentHealth <= 0 && !_isDead)
            {
                _animator.SetTrigger(AnimationStates.Die.ToString());
                _isDead = true;
            }
            else if (_currentHealth > 0) _animator.SetTrigger(AnimationStates.GetDamage.ToString());

            if (_currentHealth <= 0 && _isDead)
                EnemyDeath();
            else
                Invoke(nameof(EnableEnemy), _damageTime);
        }

        public void SetTarget(GameObject target) => _target = target;

        private void Attack()
        {
            _animator.SetTrigger(AnimationStates.Attack.ToString());
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

        #region UnityMethods

        private void Awake() => _animator = GetComponent<Animator>();

        private void Start()
        {
            _currentHealth = EnemyStats.MaxHealth.GetValue();
            _enemyDamage   = EnemyStats.Damage.GetValue();
            _speed         = EnemyStats.MoveSpeed.GetValue();
        }

        private void FixedUpdate()
        {
            _timer += Time.fixedDeltaTime;
            if (!_isBusy && _target)
            {
                transform.LookAt(_target.transform.position);

                if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position, _stopDistance))
                {
                    _animator.SetBool(AnimationStates.IsMoving.ToString(), true);
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
            else
                _animator.SetBool(AnimationStates.IsMoving.ToString(), false);
        }

        #endregion
    }
}