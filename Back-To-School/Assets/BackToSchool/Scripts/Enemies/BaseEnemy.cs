using System;
using Assets.BackToSchool.Scripts.Enums;
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

        [SerializeField] protected float _startChasingDistance = 10f;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _enemyDamage;

        protected GameObject _target;
        protected Animator _animator;
        protected NavMeshAgent _agent;
        protected EnemyStates _state;

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
                _animator.SetTrigger(AnimationStates.Die.ToString());
                _isDead          = true;
                _agent.isStopped = true;
            }
            else if (_currentHealth > 0)
            {
                _animator.SetTrigger(AnimationStates.GetDamage.ToString());
                _agent.isStopped = true;
            }
        }

        public void SetTarget(GameObject target) => _target = target;

        protected void EnableEnemy()
        {
            if (_isDead) return;
            if (_target == null) return;
            transform.LookAt(_target.transform.position);
            _isBusy          = false;
            _agent.isStopped = false;
        }

        protected void OnDeath() => Died?.Invoke(this);

        protected void MoveToNextPatrolPoint()
        {
            _animator.SetBool(AnimationStates.IsMoving.ToString(), true);
            var newDestination = SpaceOperations.GeneratePositionOnField(Constants.MinXpos, Constants.MaxXpos, Constants.MinZpos,
                Constants.MaxZpos);
            _agent.SetDestination(newDestination);
        }

        protected virtual void Attack() => _animator.SetTrigger(AnimationStates.Attack.ToString());

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _agent    = GetComponent<NavMeshAgent>();
        }

        public void Initialize(CharacterStats enemyStats)
        {
            _isDead        = false;
            _isBusy        = false;
            _currentHealth = enemyStats.MaxHealth.GetValue();
            _enemyDamage   = enemyStats.Damage.GetValue();
            _agent.speed   = enemyStats.MoveSpeed.GetValue();
            _state         = EnemyStates.Patrolling;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void Update()
        {
            if (_isBusy || _isDead || !_target)
                return;

            switch (_state)
            {
                case EnemyStates.Patrolling:
                    Patrolling();
                    break;
                case EnemyStates.Chasing:
                    Chasing();
                    break;
                case EnemyStates.Attacking:
                    Attacking();
                    break;
            }
        }

        private void Patrolling()
        {
            if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position, _startChasingDistance))
            {
                if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _agent.destination,
                    _agent.stoppingDistance))
                    MoveToNextPatrolPoint();
            }
            else
            {
                _state = EnemyStates.Chasing;
                _agent.SetDestination(_target.transform.position);
            }
        }

        private void Chasing()
        {
            if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _agent.destination,
                _agent.stoppingDistance))
            {
                _state = EnemyStates.Attacking;
                _animator.SetBool(AnimationStates.IsMoving.ToString(), false);
            }
            else
            {
                if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position,
                    _startChasingDistance))
                    _state = EnemyStates.Patrolling;
                else if (_target.transform.position != _agent.destination)
                    _agent.SetDestination(_target.transform.position);
            }
        }

        private void Attacking()
        {
            if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position,
                _agent.stoppingDistance))
            {
                _agent.SetDestination(_target.transform.position);
                _state = EnemyStates.Chasing;
                _animator.SetBool(AnimationStates.IsMoving.ToString(), true);
            }
            else
            {
                _isBusy = true;
                Attack();
            }
        }
    }
}