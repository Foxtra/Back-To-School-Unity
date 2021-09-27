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
                Invoke(nameof(SpawnEnemy), _spawnInterval);
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

            Instantiate(_enemyPrefab, _enemyPos, Quaternion.identity);
        }
    }
}