using System;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IDamageable
    {
        public Action<float, int> HealthChanged;
        public Action<BaseEnemy> Died;

        public CharacterStats EnemyStats;

        [SerializeField] protected float _startChasingDistance = 10f;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _enemyDamage;

        protected GameObject _target;
        protected Animator _animator;
        protected NavMeshAgent _agent;
        protected Constants.EnemyStates _state;

        protected float _currentHealth;
        protected bool _isBusy;
        protected bool _isDead;

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            _isBusy = true;

            if (_currentHealth <= 0 && !_isDead)
            {
                _animator.SetTrigger(Constants.AnimationStates.Die.ToString());
                _isDead          = true;
                _agent.isStopped = true;
            }
            else if (_currentHealth > 0)
            {
                _animator.SetTrigger(Constants.AnimationStates.GetDamage.ToString());
                _agent.isStopped = true;
            }

            if (_currentHealth <= 0 && _isDead)
                EnemyDeath();
        }

        public void SetTarget(GameObject target) => _target = target;

        protected void EnableEnemy()
        {
            if (_isDead) return;
            _isBusy          = false;
            _agent.isStopped = false;
        }

        protected void EnemyDeath() => Died?.Invoke(this);

        protected void OnDeath() => Destroy(gameObject);

        protected void MoveToNextPatrolPoint()
        {
            _animator.SetBool(Constants.AnimationStates.IsMoving.ToString(), true);
            var newDestination = SpaceOperations.GeneratePositionOnField(Constants.MinXpos, Constants.MaxXpos, Constants.MinZpos,
                Constants.MaxZpos);
            _agent.SetDestination(newDestination);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _agent    = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _currentHealth = EnemyStats.MaxHealth.GetValue();
            _enemyDamage   = EnemyStats.Damage.GetValue();
            _agent.speed   = EnemyStats.MoveSpeed.GetValue();
            _state         = Constants.EnemyStates.Patrolling;
        }
    }
}