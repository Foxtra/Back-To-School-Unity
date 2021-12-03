using System;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Game
{
    public interface IEnemySpawner
    {
        public event Action<int> EnemyDied;
        void SetMaxWarriorEnemies(int maxEnemiesNumber);
        void SetMaxShamanEnemies(int maxEnemiesNumber);
        void SetEnemyDamage(int enemyDamage);
        void SetEnemyMaxHealth(int maxHeath);
        void SetEnemyMoveSpeed(int moveSpeed);
        void SetTarget(GameObject target);
        void InitializeEnemyPools();
    }
}