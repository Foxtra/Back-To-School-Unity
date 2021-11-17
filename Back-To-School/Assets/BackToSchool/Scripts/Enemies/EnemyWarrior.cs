using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Utils;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyWarrior : BaseEnemy, IAttackable
    {
        public void Attack(float damage)
        {
            _animator.SetTrigger(Constants.AnimationStates.Attack.ToString());
            _target.GetComponent<IDamageable>().TakeDamage(damage);
        }

        private void Update()
        {
            if (!_isBusy && !_isDead && _target)
            {
                switch (_state)
                {
                    case Constants.EnemyStates.Patrolling:
                        if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position, _startChasingDistance))
                        {
                            if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _agent.destination,
                                _agent.stoppingDistance))
                                MoveToNextPatrolPoint();
                        }
                        else
                        {
                            _state = Constants.EnemyStates.Chasing;
                            _agent.SetDestination(_target.transform.position);
                        }

                        break;
                    case Constants.EnemyStates.Chasing:
                        if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _agent.destination,
                            _agent.stoppingDistance))
                        {
                            _state = Constants.EnemyStates.Attacking;
                            _animator.SetBool(Constants.AnimationStates.IsMoving.ToString(), false);
                        }
                        else
                        {
                            if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position,
                                _startChasingDistance))
                                _state = Constants.EnemyStates.Patrolling;
                            else if (_target.transform.position != _agent.destination)
                                _agent.SetDestination(_target.transform.position);
                        }

                        break;
                    case Constants.EnemyStates.Attacking:
                        if (!SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position,
                            _agent.stoppingDistance))
                        {
                            _agent.SetDestination(_target.transform.position);
                            _state = Constants.EnemyStates.Chasing;
                            _animator.SetBool(Constants.AnimationStates.IsMoving.ToString(), true);
                        }
                        else
                        {
                            _isBusy = true;
                            Attack(_enemyDamage);
                        }

                        break;
                }
            }
        }
    }
}