using System;
using System.Collections.Generic;
using System.Linq;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        public event Action<int> EnemyDied;

        [SerializeField] private EnemyWarrior _enemyWarriorPrefab;
        [SerializeField] private EnemyShaman _enemyShamanPrefab;
        [SerializeField] private float _maxRangeToPlayer = 10f;
        [SerializeField] private float _spawnInterval = 1f;
        [SerializeField] private int _maxWarriorEnemies = 3;
        [SerializeField] private int _maxShamanEnemies = 3;

        private Dictionary<EnemyTypes, List<GameObject>> _enemyPools = new Dictionary<EnemyTypes, List<GameObject>>();
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
            foreach (var enemy in _enemyPools.Keys.SelectMany(key => _enemyPools[key]))
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

        public void InitializeEnemyPools()
        {
            _enemyPools[EnemyTypes.EnemyWarrior] = FillEnemyList(_enemyWarriorPrefab, _maxWarriorEnemies);
            _enemyPools[EnemyTypes.EnemyShaman]  = FillEnemyList(_enemyShamanPrefab, _maxShamanEnemies);
        }

        private List<GameObject> FillEnemyList(BaseEnemy prefab, int size)
        {
            var objectPool = new List<GameObject>();

            for (var i = 0; i < size; i++)
            {
                var obj = Instantiate(prefab).gameObject;
                obj.SetActive(false);
                obj.GetComponent<BaseEnemy>().Died += ReduceEnemyCount;
                objectPool.Add(obj);
            }

            return objectPool;
        }

        private GameObject GetAvailableEnemyFromPool(EnemyTypes type)
        {
            var enemy = _enemyPools[type].Find(enemy => !enemy.activeSelf);
            return enemy;
        }

        private void ControlEnemiesCount()
        {
            if (_currentNumberOfWarriors < _maxWarriorEnemies)
            {
                SpawnEnemy(EnemyTypes.EnemyWarrior);
                _currentNumberOfWarriors++;
            }

            if (_currentNumberOfShamans < _maxShamanEnemies)
            {
                SpawnEnemy(EnemyTypes.EnemyShaman);
                _currentNumberOfShamans++;
            }
        }

        private void SpawnEnemy(EnemyTypes enemyType)
        {
            do
            {
                _enemyPos = SpaceOperations.GeneratePositionOnField(Constants.MinXpos, Constants.MaxXpos, Constants.MinZpos,
                    Constants.MaxZpos);
            }
            while (SpaceOperations.CheckIfTwoObjectsClose(_enemyPos, _target.transform.position, _maxRangeToPlayer));

            var enemyObj = GetAvailableEnemyFromPool(enemyType);
            enemyObj.SetActive(true);
            enemyObj.transform.position = _enemyPos;
            var enemy = enemyObj.GetComponent<BaseEnemy>();
            enemy.SetTarget(_target);
            enemy.Initialize(new CharacterStats(_enemyDamage, _enemyMaxHealth, _enemyMoveSpeed));
        }

        private void ReduceEnemyCount(BaseEnemy sender)
        {
            var type = EnemyTypes.EnemyWarrior;

            if (sender is EnemyWarrior)
                _currentNumberOfWarriors--;

            if (sender is EnemyShaman)
            {
                _currentNumberOfShamans--;
                type = EnemyTypes.EnemyShaman;
            }

            var enemyObj = _enemyPools[type].Find(enemy => enemy.GetComponent<BaseEnemy>().Equals(sender));
            enemyObj.SetActive(false);
            EnemyDied?.Invoke(_experienceForEnemy);
        }
    }
}