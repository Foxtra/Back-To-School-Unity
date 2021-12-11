using System;
using System.Collections.Generic;
using System.Linq;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.Game;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        public event Action<Enemy> EnemyDied;
        public event Action<int> ExperienceForEnemyGot;

        private Dictionary<EEnemyTypes, List<IEnemy>> _enemyPools = new Dictionary<EEnemyTypes, List<IEnemy>>();
        private Transform _target;
        private Vector3 _enemyPos = Vector3.zero;
        private IResourceManager _resourceManager;

        private float _xPos;
        private float _yPos = 0f;
        private float _zPos;
        private float _timer;
        private float _maxRangeToPlayer;
        private float _spawnInterval;
        private int _currentNumberOfWarriors;
        private int _currentNumberOfShamans;
        private int _maxWarriorEnemies;
        private int _maxShamanEnemies;
        private int _enemyMoveSpeed;
        private int _experienceForEnemy;

        public void SetMaxWarriorEnemies(int maxEnemiesNumber) => _maxWarriorEnemies = maxEnemiesNumber;
        public void SetMaxShamanEnemies(int maxEnemiesNumber)  => _maxShamanEnemies = maxEnemiesNumber;

        public void SetTarget(Transform target)
        {
            _target = target;
            foreach (var enemy in _enemyPools.Keys.SelectMany(key => _enemyPools[key]))
                enemy.SetTarget(_target);
        }

        public void Initialize(Transform target, IResourceManager resourceManager)
        {
            SetTarget(target);
            _resourceManager = resourceManager;

            _enemyPools[EEnemyTypes.EnemyWarrior] = FillEnemyList(EEnemyTypes.EnemyWarrior, _maxWarriorEnemies);
            _enemyPools[EEnemyTypes.EnemyShaman]  = FillEnemyList(EEnemyTypes.EnemyShaman, _maxShamanEnemies);
        }

        private void Awake()
        {
            _maxRangeToPlayer   = Constants.MaxRangeToPlayer;
            _spawnInterval      = Constants.SpawnInterval;
            _maxWarriorEnemies  = Constants.MaxWarriorEnemies;
            _maxShamanEnemies   = Constants.MaxShamanEnemies;
            _enemyMoveSpeed     = Constants.EnemyMoveSpeed;
            _experienceForEnemy = Constants.ExperienceForEnemy;
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

        private List<IEnemy> FillEnemyList(EEnemyTypes enemyType, int size)
        {
            var objectPool = new List<IEnemy>();

            for (var i = 0; i < size; i++)
            {
                var obj = _resourceManager.CreateEnemy(enemyType);
                obj.gameObject.SetActive(false);
                obj.Died += ReduceEnemyCount;
                objectPool.Add(obj);
            }

            return objectPool;
        }

        private IEnemy GetAvailableEnemyFromPool(EEnemyTypes type)
        {
            var enemy = _enemyPools[type].Find(enemy => !enemy.gameObject.activeSelf);
            return enemy;
        }

        private void ControlEnemiesCount()
        {
            if (_currentNumberOfWarriors < _maxWarriorEnemies)
            {
                SpawnEnemy(EEnemyTypes.EnemyWarrior);
                _currentNumberOfWarriors++;
            }

            if (_currentNumberOfShamans < _maxShamanEnemies)
            {
                SpawnEnemy(EEnemyTypes.EnemyShaman);
                _currentNumberOfShamans++;
            }
        }

        private void SpawnEnemy(EEnemyTypes enemyType)
        {
            do
            {
                _enemyPos = SpaceOperations.GeneratePositionOnField(Constants.MinXpos, Constants.MaxXpos, Constants.MinZpos,
                    Constants.MaxZpos);
            }
            while (SpaceOperations.CheckIfTwoObjectsClose(_enemyPos, _target.transform.position, _maxRangeToPlayer));

            var enemy = GetAvailableEnemyFromPool(enemyType);
            enemy.gameObject.SetActive(true);
            enemy.gameObject.transform.position = _enemyPos;
            enemy.SetTarget(_target);
            switch (enemyType)
            {
                case EEnemyTypes.EnemyWarrior:
                    enemy.Initialize(new CharacterStats(Constants.EnemyWarriorDamage, Constants.EnemyWarriorMaxHealth, _enemyMoveSpeed),
                        _resourceManager);
                    break;
                case EEnemyTypes.EnemyShaman:
                    enemy.Initialize(new CharacterStats(Constants.EnemyShamanDamage, Constants.EnemyShamanMaxHealth, _enemyMoveSpeed),
                        _resourceManager);
                    break;
            }
        }

        private void ReduceEnemyCount(Enemy sender)
        {
            var type = EEnemyTypes.EnemyWarrior;

            if (sender is EnemyWarrior)
                _currentNumberOfWarriors--;

            if (sender is EnemyShaman)
            {
                _currentNumberOfShamans--;
                type = EEnemyTypes.EnemyShaman;
            }

            var enemyObj = _enemyPools[type].Find(enemy => enemy.gameObject.GetComponent<Enemy>().Equals(sender));
            enemyObj.gameObject.SetActive(false);
            ExperienceForEnemyGot?.Invoke(_experienceForEnemy);
            EnemyDied?.Invoke(sender);
        }
    }
}