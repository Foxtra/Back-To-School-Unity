using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Inputs;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Progression;
using Assets.BackToSchool.Scripts.Stats;
using Assets.BackToSchool.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private HUDPresenter _hudPresenter;
        [SerializeField] private GameOverPresenter _gameOverPresenter;
        [SerializeField] private PausePresenter _pausePresenter;

        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private GameObject _inputManagerPrefab;
        [SerializeField] private GameObject _enemySpawnerPrefab;
        [SerializeField] private Camera _mainCamera;

        private PlayerController _player;
        private PlayerStats _playerStats;
        private EnemySpawner _enemySpawner;
        private IInputManager _inputManager;
        private StatsManager _statsManager;
        private SaveSystem _saveSystem;
        private LevelSystem _levelSystem;
        private PlayerInputProvider _playerInput;

        private float _gameOverDelay = 1f;
        private bool _isGamePaused;
        private bool _isPlayerDead;

        public void Initialize(SaveSystem saveSystem, GameParameters parameters)
        {
            _saveSystem   = saveSystem;
            _playerStats  = new PlayerStats();
            _levelSystem  = new LevelSystem();
            _statsManager = new StatsManager();

            _player       = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
            _inputManager = Instantiate(_inputManagerPrefab, transform.position, Quaternion.identity).GetComponent<InputManager>();
            _enemySpawner = Instantiate(_enemySpawnerPrefab, transform.position, Quaternion.identity).GetComponent<EnemySpawner>();

            SubscribeEvents();
            _statsManager.Initialize(_playerStats, parameters.InitialLevel);

            _mainCamera.GetComponent<CameraFollow>().SetTarget(_player.transform);
            _enemySpawner.SetTarget(_player.gameObject);

            _playerInput = new PlayerInputProvider(_mainCamera);
            _inputManager.Subscribe(_playerInput);
            _player.Initialize(_playerInput, _playerStats);

            var pauseInput = new PauseInputProvider();
            _inputManager.Subscribe(pauseInput);
            pauseInput.Cancelled += OnGameStopped;

            if (parameters.IsNewGame)
            {
                _saveSystem.ResetPlayerProgress();
                _player.InitializeHealth();
                _player.WeaponController.InitializeAmmo();
                _player.WeaponController.InitializeWeapon();
            }
            else
                _saveSystem.LoadPlayerProgress(_player, _levelSystem);
        }

        private void SubscribeEvents()
        {
            _gameOverPresenter.Restarted += RestartGame;
            _pausePresenter.Restarted    += RestartGame;
            _pausePresenter.Continued    += ContinueGame;

            _player.WeaponController.AmmoChanged   += _hudPresenter.OnAmmoChanged;
            _player.WeaponController.WeaponChanged += _hudPresenter.OnWeaponChanged;
            _player.Died                           += OnPlayerDeath;
            _player.HealthChanged                  += _hudPresenter.OnHealthChanged;
            _enemySpawner.EnemyDied                += _levelSystem.AddExperience;

            _levelSystem.LevelChanged      += _statsManager.LevelUp;
            _levelSystem.LevelChanged      += _hudPresenter.OnLevelChanged;
            _levelSystem.ExperienceChanged += _hudPresenter.OnExpChanged;
            _levelSystem.ProgressChanged   += () => _saveSystem.SavePlayerProgress(_player, _levelSystem);

            _statsManager.ArmorChanged     += _hudPresenter.OnArmorChanged;
            _statsManager.DamageChanged    += _hudPresenter.OnDamageChanged;
            _statsManager.MaxAmmoChanged   += _hudPresenter.OnMaxAmmoChanged;
            _statsManager.MaxHealthChanged += _hudPresenter.OnMaxHealthChanged;
            _statsManager.MoveSpeedChanged += _hudPresenter.OnMoveSpeedChanged;
        }

        #region GameHandlers

        private void OnPlayerDeath()
        {
            _isPlayerDead = true;
            _enemySpawner.SetTarget(null);
            Invoke(nameof(EndGame), _gameOverDelay);
        }

        private void StopGame()
        {
            Time.timeScale = 0f;
            _isGamePaused  = true;
            _playerInput.SetIsPause(_isGamePaused);
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
            _playerInput.SetIsPause(_isGamePaused);
            _pausePresenter.TogglePausePanel(_isGamePaused);
        }

        private void EndGame() { _gameOverPresenter.ShowGameOverPanel(); }

        private void RestartGame()
        {
            if (_isGamePaused) ContinueGame();
            _saveSystem.ResetPlayerProgress();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #endregion
    }
}