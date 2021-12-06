using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Inputs;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.Game;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Player;
using Assets.BackToSchool.Scripts.Progression;
using Assets.BackToSchool.Scripts.Stats;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.Models
{
    public class GameModel : BaseModel
    {
        private IHUDPresenter _hudPresenter;
        private IGameOverPresenter _gameOverPresenter;
        private IPausePresenter _pausePresenter;
        private ICompleteLevelPresenter _completeLevelPresenter;

        private IGameManager _gameManager;
        private ISaveSystem _saveSystem;
        private IInputManager _inputManager;
        private IResourceManager _resourceManager;

        private IStatsManager _statsManager;
        private ILevelSystem _levelSystem;
        private IObjectiveSystem _objectiveSystem;

        private IEnemySpawner _enemySpawner;

        private IPlayerController _player;
        private IPlayerInput _playerInput;
        private PlayerStats _playerStats;
        private PlayerData _playerData;

        private Camera _mainCamera;

        private bool _isGamePaused;
        private bool _isPlayerDead;

        public GameModel(ISaveSystem saveSystem, IGameManager gameManager, IResourceManager resourceManager,
            IInputManager inputManager, IViewFactory viewFactory, Camera playerCamera, IAudioManager audioManager, StartParameters parameters)
        {
            _hudPresenter           = viewFactory.CreateView<IHUDPresenter, EViews>(EViews.HUD);
            _gameOverPresenter      = viewFactory.CreateView<IGameOverPresenter, EViews>(EViews.GameOver);
            _pausePresenter         = viewFactory.CreateView<IPausePresenter, EViews>(EViews.Pause);
            _completeLevelPresenter = viewFactory.CreateView<ICompleteLevelPresenter, EViews>(EViews.CompleteLevel);

            _saveSystem      = saveSystem;
            _gameManager     = gameManager;
            _resourceManager = resourceManager;
            _inputManager    = inputManager;
            _mainCamera      = playerCamera;
            _playerStats     = new PlayerStats();
            _statsManager    = new StatsManager();
            _levelSystem     = new LevelSystem();
            _enemySpawner    = _resourceManager.CreateEnemySpawner();
            _playerData      = parameters.IsNewGame ? new PlayerData() : _saveSystem.LoadPlayerProgress();

            _playerInput = new PlayerInputProvider(_mainCamera);
            _inputManager.Subscribe(_playerInput);

            _player = _resourceManager.CreatePlayer(_playerInput, _playerStats, _playerData, audioManager);

            _objectiveSystem = new ObjectiveSystem();
            var objectives = parameters.IsNewGame
                ? new ObjectiveParameters(parameters.GameMode)
                : _saveSystem.LoadObjectiveProgress();

            SubscribeEvents();
            _statsManager.Initialize(_playerStats, _playerData.PlayerLevel);
            _levelSystem.Initialize(_playerData.PlayerLevel, _playerData.PlayerExperience);
            _objectiveSystem.Initialize(objectives);
            _hudPresenter.InitializeObjectives(objectives);

            _playerInput = new PlayerInputProvider(_mainCamera);
            _inputManager.Subscribe(_playerInput);
            _mainCamera.GetComponent<CameraFollow>().SetTarget(_player.gameObject.transform);
            _enemySpawner.InitializeEnemyPools();
            _enemySpawner.SetTarget(_player.gameObject.transform);
            _enemySpawner.SetAudioManager(audioManager);

            var pauseInput = new PauseInputProvider();
            _inputManager.Subscribe(pauseInput);
            pauseInput.Cancelled += OnGamePaused;

            _player.UpdateHUD();
        }

        private void SubscribeEvents()
        {
            _gameOverPresenter.Restarted += RestartGame;
            _pausePresenter.Restarted    += RestartGame;
            _pausePresenter.Continued    += ContinueGame;
            _pausePresenter.MenuReturned += ReturnToMenu;

            _completeLevelPresenter.MenuReturned += ReturnToMenu;
            _completeLevelPresenter.Restarted    += RestartGame;

            _player.AmmoChanged    += _hudPresenter.OnAmmoChanged;
            _player.WeaponChanged  += _hudPresenter.OnWeaponChanged;
            _player.MaxAmmoChanged += _hudPresenter.OnMaxAmmoChanged;
            _player.Died           += OnPlayerDeath;
            _player.HealthChanged  += _hudPresenter.OnHealthChanged;

            _enemySpawner.EnemyDied              += _objectiveSystem.CountEnemyDeath;
            _objectiveSystem.ObjectivesCompleted += CompleteLevel;
            _objectiveSystem.EnemiesKilled       += _hudPresenter.OnEnemiesKillChanged;
            _objectiveSystem.TimeSurvivedChanged += _hudPresenter.OnTimeChanged;

            _enemySpawner.ExperienceForEnemyGot += _levelSystem.AddExperience;
            _levelSystem.LevelChanged           += _statsManager.LevelUp;
            _levelSystem.LevelChanged           += _hudPresenter.OnLevelChanged;
            _levelSystem.ExperienceChanged      += _hudPresenter.OnExpChanged;
            _levelSystem.ProgressChanged        += SaveGame;

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

        public override void Dispose()
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

        private async void OnPlayerDeath()
        {
            _isPlayerDead = true;
            _enemySpawner.SetTarget(null);
            await UniTask.Delay(Constants.GameOverDelay);
            EndGame();
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

        private void EndGame() => _gameOverPresenter.ShowView();

        private void CompleteLevel()
        {
            StopTime();
            _playerInput.TogglePause(true);
            _completeLevelPresenter.ShowView();
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