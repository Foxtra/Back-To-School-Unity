using Assets.BackToSchool.Scripts.Interfaces.Components;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyWarrior : BaseEnemy, IMeleeAttackable
    {
        [SerializeField] private EnemyMelee _enemyMeleeWeapon;

        public void DoDamage() => _target.GetComponent<IDamageable>().TakeDamage(_enemyDamage);

        protected override void Awake()
        {
            base.Awake();
            _enemyMeleeWeapon.PlayerDamaged += DoDamage;
        }

        private void OnDestroy() => _enemyMeleeWeapon.PlayerDamaged -= DoDamage;
    }
}