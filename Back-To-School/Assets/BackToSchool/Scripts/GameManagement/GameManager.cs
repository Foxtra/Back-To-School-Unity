using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        //UI Presenter
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _PausePanel;
        [SerializeField] private Button _pauseRestartButton;
        [SerializeField] private Button _pauseContinueButton;
        private Button _gameOverRestartButton;
        //UI Presenter

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _inputManagerPrefab;
        [SerializeField] private GameObject _enemySpawnerPrefab;
        [SerializeField] private Camera _mainCamera;

        //Player class
        private GameObject _player;
        private PlayerInteracting _playerInteracting;
        private PlayerMovement _playerMovement;
        private PlayerShooting _playerShooting;
        //Player class

        private EnemySpawner _enemySpawner;
        private InputManager _inputManager;

        private float _gameOverDelay = 1f;

        private bool _isGamePaused;
        private bool _isPlayerDead;

        private void Start()
        {
            CreateGameInstances();

            //UI Presenter
            _gameOverRestartButton = _gameOverPanel.GetComponentInChildren<Button>();
            _gameOverRestartButton.onClick.AddListener(RestartGame);

            _pauseRestartButton.onClick.AddListener(RestartGame);
            _pauseContinueButton.onClick.AddListener(ContinueGame);
            //UI Presenter

            //Player class
            _playerInteracting = _player.GetComponent<PlayerInteracting>();
            _playerMovement    = _player.GetComponent<PlayerMovement>();
            _playerShooting    = _player.GetComponent<PlayerShooting>();

            _playerInteracting.Death += OnPlayerDeath;
            //Player class

            //InputManager
            _inputManager.Moved   += OnPlayerMove;
            _inputManager.Rotated += OnPlayerRotate;
            _inputManager.Stopped += OnPlayerStop;

            _inputManager.Fired    += OnPlayerFire;
            _inputManager.Reloaded += OnPlayerReloaded;

            _inputManager.Canceled += OnGameStopped;
            //InputManager
        }

        private void CreateGameInstances()
        {
            _player = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
            _mainCamera.GetComponent<CameraFollow>().SetTarget(_player.transform);
            _inputManager = Instantiate(_inputManagerPrefab, transform.position, Quaternion.identity).GetComponent<InputManager>();
            _inputManager.SetCamera(_mainCamera);
            _enemySpawner = Instantiate(_enemySpawnerPrefab, transform.position, Quaternion.identity).GetComponent<EnemySpawner>();
            _enemySpawner.SetTarget(_player);
        }

        private void OnPlayerReloaded()
        {
            if (!_isPlayerDead) _playerShooting.Reload();
        }

        private void OnPlayerFire()
        {
            if (!_isPlayerDead) _playerShooting.Fire();
        }

        private void OnGameStopped()
        {
            if (_isGamePaused) { ContinueGame(); }
            else { StopGame(); }
        }

        private void OnPlayerStop() { _playerMovement.Stop(); }

        private void OnPlayerRotate(RaycastHit rayCastHit)
        {
            if (!_isPlayerDead) _playerMovement.Rotate(rayCastHit);
        }

        private void OnPlayerMove(Vector3 direction)
        {
            if (!_isPlayerDead) _playerMovement.Move(direction);
        }

        private void StopGame()
        {
            Time.timeScale = 0f;
            _PausePanel.SetActive(true);
            _isGamePaused = true;
        }

        private void ContinueGame()
        {
            Time.timeScale = 1f;
            _PausePanel.SetActive(false);
            _isGamePaused = false;
        }

        private void OnPlayerDeath()
        {
            _isPlayerDead = true;
            _enemySpawner.SetTarget(null);
            Invoke(nameof(EndGame), _gameOverDelay);
        }

        private void EndGame() { _gameOverPanel.SetActive(true); }

        private void RestartGame()
        {
            if (_isGamePaused) ContinueGame();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}