using System;
using Assets.BackToSchool.Scripts.Interfaces.Components;


namespace Assets.BackToSchool.Scripts.Interfaces.Game
{
    public interface IEnemySpawner : ITargetable
    {
        public event Action<int> EnemyDied;
        void SetMaxWarriorEnemies(int maxEnemiesNumber);
        void SetMaxShamanEnemies(int maxEnemiesNumber);
        void SetEnemyDamage(int enemyDamage);
        void SetEnemyMaxHealth(int maxHeath);
        void SetEnemyMoveSpeed(int moveSpeed);
        void InitializeEnemyPools();
    }
}