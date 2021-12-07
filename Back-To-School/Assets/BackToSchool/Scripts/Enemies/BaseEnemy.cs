using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Game;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IBaseEnemy
    {
        public Action<float, int> HealthChanged;
        public Action<BaseEnemy> Died;

        protected float _startChasingDistance;
        protected int _maxHealth;
        protected int _enemyDamage;

        protected Transform _target;
        protected Animator _animator;
        protected NavMeshAgent _agent;
        protected EEnemyStates _state;

        protected float _currentHealth;
        protected bool _isBusy;
        protected bool _isDead;

        public void Initialize(CharacterStats enemyStats)
        {
            _isDead        = false;
            _isBusy        = false;
            _currentHealth = enemyStats.MaxHealth.GetValue();
            _maxHealth     = enemyStats.MaxHealth.GetValue();
            _enemyDamage   = enemyStats.Damage.GetValue();
            _agent.speed   = enemyStats.MoveSpeed.GetValue();
            _state         = EEnemyStates.Patrolling;

            _startChasingDistance = Constants.EnemyStartChasingDistance;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public virtual void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth, _maxHealth);
            _isBusy = true;

            if (_currentHealth <= 0 && !_isDead)
            {
                _animator.SetTrigger(EAnimTriggers.Die.ToStringCached());

                var animTime = Array.Find(_animator.runtimeAnimatorController.animationClips,
                    clip => clip.name == EEnemyAnimNames.Die.ToStringCached()).length;
                WaitWhileBusy(Mathf.RoundToInt(animTime * Constants.MillisecondsMultiplier));

                _isDead          = true;
                _agent.isStopped = true;
            }
        }

        public void SetTarget(Transform target) => _target = target;

        protected void EnableEnemy()
        {
            if (_isDead) return;
            if (_target == null)
                _state = EEnemyStates.Patrolling;
            else
                transform.LookAt(_target.transform.position);

            _isBusy          = false;
            _agent.isStopped = false;
        }

        protected void EnemyDeath() => Died?.Invoke(this);

        protected void MoveToNextPatrolPoint()
        {
            _animator.SetBool(EAnimTriggers.IsMoving.ToStringCached(), true);
            var newDestination = SpaceOperations.GeneratePositionOnField(Constants.MinXpos, Constants.MaxXpos, Constants.MinZpos,
                Constants.MaxZpos);
            _agent.SetDestination(newDestination);
        }

        protected virtual void Attack() => _animator.SetTrigger(EAnimTriggers.Attack.ToStringCached());

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _agent    = GetComponent<NavMeshAgent>();
        }

        protected async void WaitWhileBusy(int milSec)
        {
            await UniTask.Delay(milSec);
            if (_isDead)
                EnemyDeath();
            else
                EnableEnemy();
        }

        private void Update()
        {
            if (_isBusy || _isDead || !_target)
                return;

            switch (_state)
            {
                case EEnemyStates.Patrolling:
                    Patrolling();
                    break;
                case EEnemyStates.Chasing:
                    Chasing();
                    break;
                case EEnemyStates.Attacking:
                    Attacking();
                    break;
            }
        }

        private void Patrolling()
        {
            if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.position, _startChasingDistance))
            {
                if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _agent.destination,
                    _agent.stoppingDistance))
                    MoveToNextPatrolPoint();
            }
            else
            {
                _state = EEnemyStates.Chasing;
                _agent.SetDestination(_target.position);
            }
        }

        private void Chasing()
        {
            if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _agent.destination,
                _agent.stoppingDistance))
            {
                _state = EEnemyStates.Attacking;
                _animator.SetBool(EAnimTriggers.IsMoving.ToStringCached(), false);
            }
            else
            {
                if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.position,
                    _startChasingDistance))
                    _state = EEnemyStates.Patrolling;
                else if (_target.position != _agent.destination)
                    _agent.SetDestination(_target.position);
            }
        }

        private void Attacking()
        {
            if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.position,
                _agent.stoppingDistance))
            {
                _agent.SetDestination(_target.position);
                _state = EEnemyStates.Chasing;
                _animator.SetBool(EAnimTriggers.IsMoving.ToStringCached(), true);
            }
            else
            {
                _isBusy = true;
                Attack();
            }
        }
    }
}