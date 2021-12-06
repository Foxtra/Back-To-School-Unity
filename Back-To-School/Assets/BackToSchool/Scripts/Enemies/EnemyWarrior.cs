using System.Collections;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyWarrior : BaseEnemy, IMeleeAttackable
    {
        [SerializeField] private EnemyMelee _enemyMeleeWeapon;

        protected override void Awake()
        {
            base.Awake();
            _audioManager.Play(SoundNames.WarriorAttack);
            _enemyMeleeWeapon.PlayerDamaged += DoDamage;
        }

        private void OnDestroy() => _enemyMeleeWeapon.PlayerDamaged -= DoDamage;
    }
}