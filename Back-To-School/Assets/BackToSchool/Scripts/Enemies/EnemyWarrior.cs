using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Parameters;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyWarrior : Enemy, IMeleeAttackable
    {
        [SerializeField] private EnemyMelee _enemyMeleeWeapon;
        public void DoDamage() => _target.GetComponent<IDamageable>().TakeDamage(_enemyDamage);

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            if (_currentHealth > 0)
            {
                _animator.SetTrigger(EAnimTriggers.GetDamage.ToStringCached());

                var targetClipName = EEnemyAnimNames.GetHit.ToStringCached();

                var animTime = Array.Find(_animator.runtimeAnimatorController.animationClips,
                    clip => clip.name == targetClipName).length;
                WaitWhileBusy(Mathf.RoundToInt(animTime * Constants.Time.MillisecondsMultiplier));

                _agent.isStopped = true;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _enemyMeleeWeapon.PlayerDamaged += DoDamage;
        }

        protected override void Attack()
        {
            base.Attack();
            _audioManager.PlayEffect(ESounds.WarriorAttack);
            var targetClipName = EEnemyAnimNames.Attack01.ToStringCached();
            var animTime = Array.Find(_animator.runtimeAnimatorController.animationClips,
                clip => clip.name == targetClipName).length;
            WaitWhileBusy(Mathf.RoundToInt(animTime * Constants.Time.MillisecondsMultiplier));
        }

        private void OnDestroy() => _enemyMeleeWeapon.PlayerDamaged -= DoDamage;
    }
}