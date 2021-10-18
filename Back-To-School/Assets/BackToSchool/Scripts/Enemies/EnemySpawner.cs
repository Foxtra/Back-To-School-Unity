using System;
using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public Action<int> EnemyDied;

        [SerializeField] private Enemy _enemyPrefab;

        [SerializeField] private float _minXpos = -49f;
        [SerializeField] private float _maxXpos = 49f;
        [SerializeField] private float _minZpos = -49f;
        [SerializeField] private float _maxZpos = 49f;

        [SerializeField] private float _spawnInterval = 1f;
        [SerializeField] private float _maxRangeToPlayer = 10f;
        [SerializeField] private int _maxEnemies = 10;

        private List<Enemy> _enemies = new List<Enemy>();
        private GameObject _target;
        private Vector3 _enemyPos = Vector3.zero;

        private float _xPos;
        private float _yPos = 0f;
        private float _zPos;
        private float _timer;
        private int _currentNumberOfEnemies;
        private int _enemyDamage = 1;
        private int _enemyMaxHealth = 2;
        private int _enemyMoveSpeed = 2;
        private int _experienceForEnemy = 20;

        public void SetTarget(GameObject target)
        {
            _target = target;
            foreach (var enemy in _enemies) { enemy.GetComponent<Enemy>().SetTarget(_target); }
        }

        public void SetEnemyDamage(int enemyDamage)  => _enemyDamage = enemyDamage;
        public void SetEnemyMaxHealth(int maxHeath)  => _enemyMaxHealth = maxHeath;
        public void SetEnemyMoveSpeed(int moveSpeed) => _enemyMoveSpeed = moveSpeed;

        private void ReduceEnemyCount(Enemy sender)
        {
            _currentNumberOfEnemies--;
            var enemyIndex = _enemies.FindIndex(e => e.Equals(sender));
            _enemies[enemyIndex].Died -= ReduceEnemyCount;
            _enemies.Remove(sender);
            EnemyDied?.Invoke(_experienceForEnemy);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_currentNumberOfEnemies < _maxEnemies)
            {
                if (_timer > _spawnInterval && _target)
                {
                    _currentNumberOfEnemies++;
                    SpawnEnemy();
                    _timer = 0f;
                }
            }
        }

        private void SpawnEnemy()
        {
            do
            {
                _xPos = Random.Range(_minXpos, _maxXpos);
                _zPos = Random.Range(_minZpos, _maxZpos);

                _enemyPos = new Vector3(_xPos, _yPos, _zPos);
            } while (SpaceOperations.CheckIfTwoObjectsClose(_enemyPos, _target.transform.position, _maxRangeToPlayer));


            var enemy = Instantiate(_enemyPrefab, _enemyPos, Quaternion.identity);
            enemy.Died += ReduceEnemyCount;
            enemy.SetTarget(_target);
            enemy.EnemyStats = new CharacterStats(_enemyDamage, _enemyMaxHealth, _enemyMoveSpeed);

            _enemies.Add(enemy.GetComponent<Enemy>());
        }
    }
}