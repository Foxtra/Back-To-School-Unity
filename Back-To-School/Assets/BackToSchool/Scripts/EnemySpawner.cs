using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets.BackToSchool.Scripts
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
        [SerializeField] private int _maxEnemies = 20;

        private GameObject _player;
        private Vector3 _enemyPos = Vector3.zero;

        private float _xPos;
        private float _yPos = 0f;
        private float _zPos;
        private int _currentNumberOfEnemies;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (_currentNumberOfEnemies < _maxEnemies)
            {
                _currentNumberOfEnemies++;
                Invoke(nameof(SpawnEnemies), _spawnInterval);
            }
        }

        private void SpawnEnemies()
        {
            do
            {
                _xPos = Random.Range(_minXpos, _maxXpos);
                _zPos = Random.Range(_minZpos, _maxZpos);

                _enemyPos = new Vector3(_xPos, _yPos, _zPos);
            } while (CheckIfSpawnCloseToPlayer(_enemyPos));


            Instantiate(_enemyPrefab, _enemyPos, Quaternion.identity);
        }

        private bool CheckIfSpawnCloseToPlayer(Vector3 enemyPos)
        {
            var heading = enemyPos - _player.transform.position;

            return heading.sqrMagnitude < Math.Pow(_maxRangeToPlayer, 2);
        }
    }
}