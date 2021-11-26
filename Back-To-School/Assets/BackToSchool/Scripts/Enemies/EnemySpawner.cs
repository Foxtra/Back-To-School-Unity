using System;
using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public Action<int> EnemyDied;

        [SerializeField] private EnemyWarrior _enemyWarriorPrefab;
        [SerializeField] private EnemyShaman _enemyShamanPrefab;
        [SerializeField] private float _maxRangeToPlayer = 10f;
        [SerializeField] private float _spawnInterval = 1f;
        [SerializeField] private int _maxWarriorEnemies = 3;
        [SerializeField] private int _maxShamanEnemies = 3;

        private List<BaseEnemy> _enemies = new List<BaseEnemy>();
        private GameObject _target;
        private Vector3 _enemyPos = Vector3.zero;

        private float _xPos;
        private float _yPos = 0f;
        private float _zPos;
        private float _timer;
        private int _currentNumberOfWarriors;
        private int _currentNumberOfShamans;
        private int _enemyDamage = 2;
        private int _enemyMaxHealth = 2;
        private int _enemyMoveSpeed = 2;
        private int _experienceForEnemy = 20;

        public void SetMaxWarriorEnemies(int maxEnemiesNumber) => _maxWarriorEnemies = maxEnemiesNumber;
        public void SetMaxShamanEnemies(int maxEnemiesNumber)  => _maxShamanEnemies = maxEnemiesNumber;
        public void SetEnemyDamage(int enemyDamage)            => _enemyDamage = enemyDamage;
        public void SetEnemyMaxHealth(int maxHeath)            => _enemyMaxHealth = maxHeath;
        public void SetEnemyMoveSpeed(int moveSpeed)           => _enemyMoveSpeed = moveSpeed;

        public void SetTarget(GameObject target)
        {
            _target = target;
            foreach (var enemy in _enemies)
                enemy.GetComponent<BaseEnemy>().SetTarget(_target);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_currentNumberOfWarriors >= _maxWarriorEnemies && _currentNumberOfShamans >= _maxShamanEnemies)
                return;
            if (!(_timer > _spawnInterval) || !_target)
                return;

            ControlEnemiesCount();
            _timer = 0f;
        }

        private void ControlEnemiesCount()
        {
            if (_currentNumberOfWarriors < _maxWarriorEnemies)
            {
                SpawnEnemy(_enemyWarriorPrefab);
                _currentNumberOfWarriors++;
            }

            if (_currentNumberOfShamans < _maxShamanEnemies)
            {
                SpawnEnemy(_enemyShamanPrefab);
                _currentNumberOfShamans++;
            }
        }

        private void SpawnEnemy(BaseEnemy baseEnemy)
        {
            do
            {
                _enemyPos = SpaceOperations.GeneratePositionOnField(Constants.MinXpos, Constants.MaxXpos, Constants.MinZpos,
                    Constants.MaxZpos);
            }
            while (SpaceOperations.CheckIfTwoObjectsClose(_enemyPos, _target.transform.position, _maxRangeToPlayer));


            var enemy = Instantiate(baseEnemy, _enemyPos, Quaternion.identity);
            enemy.Died += ReduceEnemyCount;
            enemy.SetTarget(_target);
            enemy.EnemyStats = new CharacterStats(_enemyDamage, _enemyMaxHealth, _enemyMoveSpeed);

            _enemies.Add(enemy.GetComponent<BaseEnemy>());
        }

        private void ReduceEnemyCount(BaseEnemy sender)
        {
            if (sender is EnemyWarrior)
                _currentNumberOfWarriors--;
            if (sender is EnemyShaman)
                _currentNumberOfShamans--;

            var enemyIndex = _enemies.FindIndex(e => e.Equals(sender));
            _enemies[enemyIndex].Died -= ReduceEnemyCount;
            _enemies.Remove(sender);
            EnemyDied?.Invoke(_experienceForEnemy);
        }

        private void OnDestroy()
        {
            foreach (var enemy in _enemies)
                enemy.Died -= ReduceEnemyCount;
        }
    }
}