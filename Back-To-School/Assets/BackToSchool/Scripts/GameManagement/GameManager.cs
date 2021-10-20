using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Progression;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private HUDPresenter _hudPresenter;
        [SerializeField] private GameOverPresenter _gameOverPresenter;
        [SerializeField] private PausePresenter _pausePresenter;

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _inputManagerPrefab;
        [SerializeField] private GameObject _enemySpawnerPrefab;
        [SerializeField] private Camera _mainCamera;

        private GameObject _playerObject;
        private EnemySpawner _enemySpawner;
        private InputManager _inputManager;
        private Player.Player _player;
        private StatsManager _statsManager;
        private SaveSystem _saveSystem;

        private float _gameOverDelay = 1f;
        private bool _isGamePaused;
        private bool _isPlayerDead;

        private void Start()
        {
            CreateGameInstances();

            _gameOverPresenter.Restarted += RestartGame;
            _pausePresenter.Restarted    += RestartGame;
            _pausePresenter.Continued    += ContinueGame;

            _player.AmmoChanged                   += _hudPresenter.OnAmmoChanged;
            _player.Died                          += OnPlayerDeath;
            _player.HealthChanged                 += _hudPresenter.OnHealthChanged;
            _enemySpawner.EnemyDied               += _player.LevelSystem.AddExperience;
            _player.LevelSystem.LevelChanged      += _statsManager.OnLevelUp;
            _player.LevelSystem.LevelChanged      += _hudPresenter.OnLevelChanged;
            _player.LevelSystem.ExperienceChanged += _hudPresenter.OnExpChanged;
            _player.LevelSystem.ProgressChanged   += _saveSystem.OnPlayerProgressChanged;

            _statsManager.ArmorChanged     += _hudPresenter.OnArmorChanged;
            _statsManager.DamageChanged    += _hudPresenter.OnDamageChanged;
            _statsManager.MaxAmmoChanged   += _hudPresenter.OnMaxAmmoChanged;
            _statsManager.MaxHealthChanged += _hudPresenter.OnMaxHealthChanged;
            _statsManager.MoveSpeedChanged += _hudPresenter.OnMoveSpeedChanged;
            if (!GlobalSettings.IsNewGame && _saveSystem.IsSaveDataExists())
                _saveSystem.LoadPlayerProgress();
            else
            {
                _saveSystem.ResetPlayerProgress();
                _statsManager.OnLevelUp(0);
                _player.InitializeAmmoAndHealt();
            }


            _inputManager.Moved    += OnPlayerMove;
            _inputManager.Rotated  += OnPlayerRotate;
            _inputManager.Stopped  += OnPlayerStop;
            _inputManager.Fired    += OnPlayerFire;
            _inputManager.Reloaded += OnPlayerReloaded;
            _inputManager.Canceled += OnGameStopped;
        }

        private void CreateGameInstances()
        {
            _playerObject       = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
            _player             = _playerObject.GetComponent<Player.Player>();
            _player.PlayerStats = new PlayerStats();
            _player.LevelSystem = new LevelSystem();

            _statsManager = new StatsManager(_player);
            _saveSystem   = new SaveSystem(_player, _statsManager);
            _mainCamera.GetComponent<CameraFollow>().SetTarget(_playerObject.transform);
            _inputManager = Instantiate(_inputManagerPrefab, transform.position, Quaternion.identity).GetComponent<InputManager>();
            _inputManager.SetCamera(_mainCamera);
            _enemySpawner = Instantiate(_enemySpawnerPrefab, transform.position, Quaternion.identity).GetComponent<EnemySpawner>();
            _enemySpawner.SetTarget(_playerObject);
        }

        #region PlayerHandlers

        private void OnPlayerReloaded()
        {
            if (!(_isPlayerDead || _isGamePaused)) _player.Reload();
        }

        private void OnPlayerFire()
        {
            if (!(_isPlayerDead || _isGamePaused)) _player.Fire();
        }

        private void OnPlayerStop() => _player.Stop();

        private void OnPlayerRotate(Vector3 pointToRotate)
        {
            if (!(_isPlayerDead || _isGamePaused)) _player.Rotate(pointToRotate);
        }

        private void OnPlayerMove(Vector3 direction)
        {
            if (!(_isPlayerDead || _isGamePaused)) _player.Move(direction);
        }

        private void OnPlayerDeath()
        {
            _isPlayerDead = true;
            _enemySpawner.SetTarget(null);
            Invoke(nameof(EndGame), _gameOverDelay);
        }

        #endregion

        #region GameHandlers

        private void StopGame()
        {
            Time.timeScale = 0f;
            _isGamePaused  = true;
            _pausePresenter.TogglePausePanel(_isGamePaused);
        }

        private void OnGameStopped()
        {
            if (_isGamePaused)
                ContinueGame();
            else
                StopGame();
        }

        private void ContinueGame()
        {
            Time.timeScale = 1f;
            _isGamePaused  = false;
            _pausePresenter.TogglePausePanel(_isGamePaused);
        }

        private void EndGame() { _gameOverPresenter.ShowGameOverPanel(); }

        private void RestartGame()
        {
            if (_isGamePaused) ContinueGame();
            GlobalSettings.IsNewGame = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #endregion
    }
}