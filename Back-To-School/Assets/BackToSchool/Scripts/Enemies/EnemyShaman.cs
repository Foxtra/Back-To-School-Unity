using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Parameters;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyShaman : Enemy, IShootable
    {
        [SerializeField] private Transform _shootingPosition;

        private IBullet _fireBall;
        private float _timer;
        private float _attackInterval = Constants.ShamanAttackInterval;
        private float _bulletForce = Constants.EnemyFireBallForce;

        public void Fire()
        {
            _fireBall = _resourceManager.CreateFireBall();

            _fireBall.gameObject.transform.position = _shootingPosition.position;
            _fireBall.gameObject.transform.rotation = _shootingPosition.rotation;
            _fireBall.gameObject.transform.parent   = null;
            _fireBall.SetDamage(_enemyDamage);
            _fireBall.Launch(_bulletForce);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            if (_currentHealth > 0)
            {
                _animator.SetTrigger(EAnimTriggers.GetDamage.ToStringCached());

                var targetClipName = EEnemyAnimNames.Hit.ToStringCached();

                var animTime = Array.Find(_animator.runtimeAnimatorController.animationClips,
                    clip => clip.name == targetClipName).length;
                WaitWhileBusy(Mathf.RoundToInt(animTime * Constants.MillisecondsMultiplier));

                _agent.isStopped = true;
            }
        }

        protected override void Attack()
        {
            if (_timer > _attackInterval)
            {
                base.Attack();
                var targetClipName = EEnemyAnimNames.Spell.ToStringCached();
                var animTime = Array.Find(_animator.runtimeAnimatorController.animationClips,
                    clip => clip.name == targetClipName).length;
                WaitWhileBusy(Mathf.RoundToInt(animTime * Constants.MillisecondsMultiplier));
                Fire();
                _timer = 0f;
            }
            else
                EnableEnemy();
        }

        private void FixedUpdate() => _timer += Time.fixedDeltaTime;
    }
}