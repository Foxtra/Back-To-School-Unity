using System;
using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Interfaces.Components;


namespace Assets.BackToSchool.Scripts.Interfaces.Game
{
    public interface IEnemySpawner : ITargetable
    {
        public event Action<BaseEnemy> EnemyDied;
        public event Action<int> ExperienceForEnemyGot;
        public void SetMaxWarriorEnemies(int maxEnemiesNumber);
        public void SetMaxShamanEnemies(int maxEnemiesNumber);
        public void SetEnemyDamage(int enemyDamage);
        public void SetEnemyMaxHealth(int maxHeath);
        public void SetEnemyMoveSpeed(int moveSpeed);
        public void InitializeEnemyPools();
    }
}