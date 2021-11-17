using Assets.BackToSchool.Scripts.Interfaces;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyWarrior : BaseEnemy, IMeleeAttackable
    {
        public void DoDamage(float damage) => _target.GetComponent<IDamageable>().TakeDamage(damage);

        protected override void Attack()
        {
            base.Attack();
            DoDamage(_enemyDamage);
        }
    }
}