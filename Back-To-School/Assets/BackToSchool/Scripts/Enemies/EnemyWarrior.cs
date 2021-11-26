using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Utils;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyWarrior : BaseEnemy, IMeleeAttackable
    {
        public void DoDamage()
        {
            if (SpaceOperations.CheckIfTwoObjectsClose(transform.position, _target.transform.position,
                _agent.stoppingDistance))
                _target.GetComponent<IDamageable>().TakeDamage(_enemyDamage);
        }
    }
}