using System;
using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Stats;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Game
{
    public interface IBaseEnemy : IDamageable, ITargetable
    {
        public event Action<float, int> HealthChanged;
        public event Action<BaseEnemy> Died;
        public GameObject gameObject { get; }
        public void Initialize(CharacterStats enemyStats);
    }
}