using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Utils;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;

        [SerializeField] private float _minXpos = -49f;
        [SerializeField] private float _maxXpos = 49f;
        [SerializeField] private float _minZpos = -49f;
        [SerializeField] private float _maxZpos = 49f;

        [SerializeField] private float _spawnInterval = 1f;
        [SerializeField] private float _maxRangeToPlayer = 10f;
        [SerializeField] private int _maxEnemies = 10;

        private List<Enemy> _enemies = new List<Enemy>();
        private GameObject _player;
        private Vector3 _enemyPos = Vector3.zero;

        private float _xPos;
        private float _yPos = 0f;
        private float _zPos;
        private float _timer;
        private int _currentNumberOfEnemies;

        private void ReduceEnemyCount(Enemy sender, EnemyArgs _args)
        {
            if (_args.NewHealthValue == 0)
            {
                _currentNumberOfEnemies--;
                var enemyIndex = _enemies.FindIndex(e => e.Equals(sender));
                _enemies[enemyIndex].OnHealthChanged -= ReduceEnemyCount;
                _enemies.Remove(sender);
            }
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_currentNumberOfEnemies < _maxEnemies)
            {
                if (_timer > _spawnInterval)
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
            } while (SpaceOperations.CheckIfTwoObjectsClose(_enemyPos, _player.transform.position, _maxRangeToPlayer));


            var enemy = Instantiate(_enemyPrefab, _enemyPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().OnHealthChanged += ReduceEnemyCount;
            _enemies.Add(enemy.GetComponent<Enemy>());
        }
    }
}