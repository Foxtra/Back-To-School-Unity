using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
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
        protected EStates _state;

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
                _animator.SetTrigger(EAnimations.Die.ToString());
                _isDead          = true;
                _agent.isStopped = true;
            }
            else if (_currentHealth > 0)
            {
                _animator.SetTrigger(EAnimations.GetDamage.ToString());
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
            _animator.SetBool(EAnimations.IsMoving.ToString(), true);
            var newDestination = SpaceOperations.GeneratePositionOnField(Constants.MinXpos, Constants.MaxXpos, Constants.MinZpos,
                Constants.MaxZpos);
            _agent.SetDestination(newDestination);
        }

        protected virtual void Attack() => _animator.SetTrigger(EAnimations.Attack.ToString());

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
            _state         = EStates.Patrolling;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void Update()
        {
            if (_isBusy || _isDead || !_target)
                return;

            switch (_state)
            {
                case EStates.Patrolling:
                    Patrolling();
                    break;
                case EStates.Chasing:
                    Chasing();
                    break;
                case EStates.Attacking:
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
                _state = EStates.Chasing;
                _agent.SetDestination(_target.transform.position);
            }
        }

        private void Chasing()
        {
            if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _agent.destination,
                _agent.stoppingDistance))
            {
                _state = EStates.Attacking;
                _animator.SetBool(EAnimations.IsMoving.ToString(), false);
            }
            else
            {
                if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position,
                    _startChasingDistance))
                    _state = EStates.Patrolling;
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
                _state = EStates.Chasing;
                _animator.SetBool(EAnimations.IsMoving.ToString(), true);
            }
            else
            {
                _isBusy = true;
                Attack();
            }
        }
    }
}