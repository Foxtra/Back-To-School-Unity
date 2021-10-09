using System;
using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public event Action<int, int, int> HealthChanged;
        public event Action<Enemy> Death;

        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _stopDistance = 1f;
        [SerializeField] private float _attackInterval = 2f;
        [SerializeField] private int _maxHealth = 2;

        private GameObject _target;
        private Animator _animator;
        private PlayerInteracting _playerInteracting;

        private float _damageTime = 1.2f;
        private float _deathTime = 1.5f;
        private float _timer;
        private int _currentHealth;

        private bool _isBusy;
        private bool _isDead;

        public void GetDamage()
        {
            _currentHealth--;
            HealthChanged?.Invoke(_currentHealth, _maxHealth, 1);
            _isBusy = true;

            if (_currentHealth == 0 && !_isDead)
            {
                _animator.SetTrigger(AnimationStates.Die);
                _isDead = true;
            }
            else if (_currentHealth > 0)
            {
                _animator.SetTrigger(AnimationStates.GetDamage);
            }

            if (_currentHealth == 0 && _isDead)
            {
                EnemyDeath();
            }
            else
            {
                Invoke(nameof(EnableEnemy), _damageTime);
            }
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _animator = GetComponent<Animator>();
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
            else
            {
                _animator.SetBool(AnimationStates.IsMoving, false);
            }
        }

        private void Attack()
        {
            _animator.SetTrigger(AnimationStates.Attack);
            _playerInteracting.GetDamage();
        }

        private void EnableEnemy()
        {
            if (!_isDead)
            {
                _isBusy = false;
            }
        }

        private void EnemyDeath()
        {
            Destroy(gameObject, _deathTime);
            Death?.Invoke(this);
        }
    }
}