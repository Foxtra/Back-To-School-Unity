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
        public event Action<BaseEnemy> EnemyDied;
        public event Action<int> ExperienceForEnemyGot;

        [SerializeField] private EnemyWarrior _enemyWarriorPrefab;
        [SerializeField] private EnemyShaman _enemyShamanPrefab;

        private IAudioManager _audioManager;
        private Dictionary<EEnemyTypes, List<GameObject>> _enemyPools = new Dictionary<EEnemyTypes, List<GameObject>>();
        private Transform _target;
        private Vector3 _enemyPos = Vector3.zero;

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
        private int _enemyDamage;
        private int _enemyMaxHealth;
        private int _enemyMoveSpeed;
        private int _experienceForEnemy;

        public void SetMaxWarriorEnemies(int maxEnemiesNumber) => _maxWarriorEnemies = maxEnemiesNumber;
        public void SetMaxShamanEnemies(int maxEnemiesNumber)  => _maxShamanEnemies = maxEnemiesNumber;
        public void SetEnemyDamage(int enemyDamage)            => _enemyDamage = enemyDamage;
        public void SetEnemyMaxHealth(int maxHeath)            => _enemyMaxHealth = maxHeath;
        public void SetEnemyMoveSpeed(int moveSpeed)           => _enemyMoveSpeed = moveSpeed;

        public void SetTarget(Transform target)
        {
            _target = target;
            foreach (var enemy in _enemyPools.Keys.SelectMany(key => _enemyPools[key]))
                enemy.GetComponent<BaseEnemy>().SetTarget(_target);
        }

        public void SetAudioManager(IAudioManager audioManager) => _audioManager = audioManager;

        private void Awake()
        {
            _maxRangeToPlayer   = Constants.MaxRangeToPlayer;
            _spawnInterval      = Constants.SpawnInterval;
            _maxWarriorEnemies  = Constants.MaxWarriorEnemies;
            _maxShamanEnemies   = Constants.MaxShamanEnemies;
            _enemyDamage        = Constants.EnemyDamage;
            _enemyMaxHealth     = Constants.EnemyMaxHealth;
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

        public void InitializeEnemyPools()
        {
            _enemyPools[EEnemyTypes.EnemyWarrior] = FillEnemyList(_enemyWarriorPrefab, _maxWarriorEnemies);
            _enemyPools[EEnemyTypes.EnemyShaman]  = FillEnemyList(_enemyShamanPrefab, _maxShamanEnemies);
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

        private GameObject GetAvailableEnemyFromPool(EEnemyTypes type)
        {
            var enemy = _enemyPools[type].Find(enemy => !enemy.activeSelf);
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

            var enemyObj = GetAvailableEnemyFromPool(enemyType);
            enemyObj.SetActive(true);
            enemyObj.transform.position = _enemyPos;
            var enemy = enemyObj.GetComponent<BaseEnemy>();
            enemy.SetTarget(_target);
            enemy.SetAudioManager(_audioManager);
            enemy.Initialize(new CharacterStats(_enemyDamage, _enemyMaxHealth, _enemyMoveSpeed));
        }

        private void ReduceEnemyCount(BaseEnemy sender)
        {
            var type = EEnemyTypes.EnemyWarrior;

            if (sender is EnemyWarrior)
                _currentNumberOfWarriors--;

            if (sender is EnemyShaman)
            {
                _currentNumberOfShamans--;
                type = EEnemyTypes.EnemyShaman;
            }

            var enemyObj = _enemyPools[type].Find(enemy => enemy.GetComponent<BaseEnemy>().Equals(sender));
            enemyObj.SetActive(false);
            ExperienceForEnemyGot?.Invoke(_experienceForEnemy);
            EnemyDied?.Invoke(sender);
        }
    }
}