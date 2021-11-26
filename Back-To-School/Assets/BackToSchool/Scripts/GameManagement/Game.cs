using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Inputs;
using Assets.BackToSchool.Scripts.Interfaces;
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
        [SerializeField] private CompleteLevelPresenter _completeLevelPresenter;

        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private GameObject _inputManagerPrefab;
        [SerializeField] private GameObject _enemySpawnerPrefab;
        [SerializeField] private Camera _mainCamera;

        private GameManager _gameManager;
        private SaveSystem _saveSystem;
        private ObjectiveSystem _objectiveSystem;
        private IInputManager _inputManager;

        private StatsManager _statsManager;
        private LevelSystem _levelSystem;

        private EnemySpawner _enemySpawner;

        private PlayerController _player;
        private IPlayerInput _playerInput;
        private PlayerStats _playerStats;
        private PlayerData _playerData;

        private float _gameOverDelay = 1f;
        private bool _isGamePaused;
        private bool _isPlayerDead;

        public void Initialize(SaveSystem saveSystem, GameManager gameManager, StartParameters startParameters, IAudioManager audioManager)
        {
            _saveSystem      = saveSystem;
            _gameManager     = gameManager;
            _playerStats     = new PlayerStats();
            _statsManager    = new StatsManager();
            _playerData      = startParameters.IsNewGame ? new PlayerData() : _saveSystem.LoadPlayerProgress();
            _levelSystem     = new LevelSystem();
            _objectiveSystem = new ObjectiveSystem();
            var objectives = startParameters.IsNewGame
                ? new ObjectiveParameters(startParameters.GameMode)
                : _saveSystem.LoadObjectiveProgress();

            _player       = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
            _inputManager = Instantiate(_inputManagerPrefab, transform.position, Quaternion.identity).GetComponent<InputManager>();
            _enemySpawner = Instantiate(_enemySpawnerPrefab, transform.position, Quaternion.identity).GetComponent<EnemySpawner>();

            SubscribeEvents();
            _statsManager.Initialize(_playerStats, _playerData.PlayerLevel);
            _levelSystem.Initialize(_playerData.PlayerLevel, _playerData.PlayerExperience);
            _objectiveSystem.Initialize(objectives);
            _hudPresenter.InitializeObjectives(objectives);

            _mainCamera.GetComponent<CameraFollow>().SetTarget(_player.transform);
            _enemySpawner.SetTarget(_player.gameObject);
            _enemySpawner.SetAudioManager(audioManager);

            _playerInput = new PlayerInputProvider(_mainCamera);
            _inputManager.Subscribe(_playerInput);
            _player.Initialize(_playerInput, _playerStats, _playerData, audioManager);

            var pauseInput = new PauseInputProvider();
            _inputManager.Subscribe(pauseInput);
            pauseInput.Cancelled += OnGamePaused;
        }

        private void Update()
        {
            if (_objectiveSystem != null)
                _objectiveSystem.CountTimePassed(Time.deltaTime);
        }

        private void SubscribeEvents()
        {
            _gameOverPresenter.Restarted += RestartGame;
            _pausePresenter.Restarted    += RestartGame;
            _pausePresenter.Continued    += ContinueGame;
            _pausePresenter.MenuReturned += ReturnToMenu;

            _completeLevelPresenter.MenuReturned += ReturnToMenu;
            _completeLevelPresenter.Restarted    += RestartGame;

            _player.AmmoChanged                 += _hudPresenter.OnAmmoChanged;
            _player.WeaponChanged               += _hudPresenter.OnWeaponChanged;
            _player.MaxAmmoChanged              += _hudPresenter.OnMaxAmmoChanged;
            _player.Died                        += OnPlayerDeath;
            _player.HealthChanged               += _hudPresenter.OnHealthChanged;
            _enemySpawner.ExperienceForEnemyGot += _levelSystem.AddExperience;

            _enemySpawner.EnemyDied              += _objectiveSystem.CountEnemyDeath;
            _objectiveSystem.ObjectivesCompleted += CompleteLevel;
            _objectiveSystem.EnemiesKilled       += _hudPresenter.OnEnemiesKillChanged;
            _objectiveSystem.TimeSurvivedChanged += _hudPresenter.OnTimeChanged;

            _levelSystem.LevelChanged      += _statsManager.LevelUp;
            _levelSystem.LevelChanged      += _hudPresenter.OnLevelChanged;
            _levelSystem.ExperienceChanged += _hudPresenter.OnExpChanged;
            _levelSystem.ProgressChanged   += SaveGame;

            _statsManager.ArmorChanged     += _hudPresenter.OnArmorChanged;
            _statsManager.DamageChanged    += _hudPresenter.OnDamageChanged;
            _statsManager.MaxHealthChanged += _hudPresenter.OnMaxHealthChanged;
            _statsManager.MoveSpeedChanged += _hudPresenter.OnMoveSpeedChanged;
        }

        private void SaveGame()
        {
            _saveSystem.SavePlayerProgress(new PlayerData(_levelSystem.GetLevelNumber(), _levelSystem.GetExperience(),
                _player.GetAmmoValue(), _player.GetActiveWeaponIndex(), _player.GetHealthValue()));
            _saveSystem.SaveObjectiveProgress(_objectiveSystem.GetObjectivesProgress());
        }

        private void OnDestroy()
        {
            _gameOverPresenter.Restarted -= RestartGame;
            _pausePresenter.Restarted    -= RestartGame;
            _pausePresenter.Continued    -= ContinueGame;
            _pausePresenter.MenuReturned -= ReturnToMenu;

            _player.AmmoChanged                 -= _hudPresenter.OnAmmoChanged;
            _player.WeaponChanged               -= _hudPresenter.OnWeaponChanged;
            _player.MaxAmmoChanged              -= _hudPresenter.OnMaxAmmoChanged;
            _player.Died                        -= OnPlayerDeath;
            _player.HealthChanged               -= _hudPresenter.OnHealthChanged;
            _enemySpawner.ExperienceForEnemyGot -= _levelSystem.AddExperience;

            _enemySpawner.EnemyDied              -= _objectiveSystem.CountEnemyDeath;
            _objectiveSystem.ObjectivesCompleted -= CompleteLevel;
            _objectiveSystem.EnemiesKilled       -= _hudPresenter.OnEnemiesKillChanged;
            _objectiveSystem.TimeSurvivedChanged -= _hudPresenter.OnTimeChanged;

            _levelSystem.LevelChanged      -= _statsManager.LevelUp;
            _levelSystem.LevelChanged      -= _hudPresenter.OnLevelChanged;
            _levelSystem.ExperienceChanged -= _hudPresenter.OnExpChanged;
            _levelSystem.ProgressChanged   -= SaveGame;

            _statsManager.ArmorChanged     -= _hudPresenter.OnArmorChanged;
            _statsManager.DamageChanged    -= _hudPresenter.OnDamageChanged;
            _statsManager.MaxHealthChanged -= _hudPresenter.OnMaxHealthChanged;
            _statsManager.MoveSpeedChanged -= _hudPresenter.OnMoveSpeedChanged;
        }

        #region GameHandlers

        private void OnPlayerDeath()
        {
            _isPlayerDead = true;
            _enemySpawner.SetTarget(null);
            Invoke(nameof(EndGame), _gameOverDelay);
        }

        private void PauseGame()
        {
            StopTime();
            _playerInput.TogglePause(_isGamePaused);
            _pausePresenter.TogglePausePanel(_isGamePaused);
        }

        private void StopTime()
        {
            Time.timeScale = 0f;
            _isGamePaused  = true;
        }

        private void OnGamePaused()
        {
            if (_isGamePaused)
                ContinueGame();
            else
                PauseGame();
        }

        private void ContinueGame()
        {
            ContinueTime();
            _playerInput.TogglePause(_isGamePaused);
            _pausePresenter.TogglePausePanel(_isGamePaused);
        }

        private void ContinueTime()
        {
            Time.timeScale = 1f;
            _isGamePaused  = false;
        }

        private void EndGame() => _gameOverPresenter.ShowGameOverPanel();

        private void CompleteLevel()
        {
            StopTime();
            _playerInput.TogglePause(true);
            _completeLevelPresenter.ShowCompleteLevelPanel();
        }

        private void ReturnToMenu()
        {
            if (_isGamePaused) ContinueGame();
            SaveGame();
            _gameManager.ReturnToMenu();
        }

        private void RestartGame()
        {
            if (_isGamePaused) ContinueGame();
            _saveSystem.ResetPlayerProgress();
            _gameManager.RestartLevel(SceneManager.GetActiveScene().name, _objectiveSystem.GetObjectivesProgress().GameMode);
        }

        #endregion
    }
}