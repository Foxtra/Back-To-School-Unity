using System.Collections;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyWarrior : BaseEnemy, IMeleeAttackable
    {
        [SerializeField] private float _delayBeforeDamage = 0.5f;
        public void DoDamage(float damage) => _target.GetComponent<IDamageable>().TakeDamage(damage);

        protected override void Attack()
        {
            base.Attack();
            _audioManager.Play(SoundNames.WarriorAttack);
            StartCoroutine(nameof(ShowDamageEffect));
        }

        private IEnumerator ShowDamageEffect()
        {
            yield return new WaitForSeconds(_delayBeforeDamage);
            DoDamage(_enemyDamage);
        }
    }
}