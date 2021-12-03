using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Inputs;
using Assets.BackToSchool.Scripts.Interfaces;
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

        private IGameManager _gameManager;
        private ISaveSystem _saveSystem;
        private IInputManager _inputManager;
        private IResourceManager _resourceManager;

        private IStatsManager _statsManager;
        private ILevelSystem _levelSystem;

        private IEnemySpawner _enemySpawner;

        private IPlayerController _player;
        private IPlayerInput _playerInput;
        private PlayerStats _playerStats;
        private PlayerData _playerData;

        private Camera _mainCamera;

        private bool _isGamePaused;
        private bool _isPlayerDead;

        public GameModel(ISaveSystem saveSystem, IGameManager gameManager, IResourceManager resourceManager,
            IInputManager inputManager, IViewFactory viewFactory, Camera playerCamera, StartParameters parameters)
        {
            _hudPresenter      = viewFactory.CreateView<IHUDPresenter, EViews>(EViews.HUD);
            _gameOverPresenter = viewFactory.CreateView<IGameOverPresenter, EViews>(EViews.GameOver);
            _pausePresenter    = viewFactory.CreateView<IPausePresenter, EViews>(EViews.Pause);

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

            _player = _resourceManager.CreatePlayer(_playerInput, _playerStats, _playerData);

            SubscribeEvents();
            _statsManager.Initialize(_playerStats, _playerData.PlayerLevel);
            _levelSystem.Initialize(_playerData.PlayerLevel, _playerData.PlayerExperience);

            _mainCamera.GetComponent<CameraFollow>().SetTarget(_player.gameObject.transform);
            _enemySpawner.InitializeEnemyPools();
            _enemySpawner.SetTarget(_player.gameObject.transform);

            var pauseInput = new PauseInputProvider();
            _inputManager.Subscribe(pauseInput);
            pauseInput.Cancelled += OnGamePaused;
        }

        private void SubscribeEvents()
        {
            _gameOverPresenter.Restarted += RestartGame;
            _pausePresenter.Restarted    += RestartGame;
            _pausePresenter.Continued    += ContinueGame;
            _pausePresenter.MenuReturned += ReturnToMenu;

            _player.AmmoChanged     += _hudPresenter.OnAmmoChanged;
            _player.WeaponChanged   += _hudPresenter.OnWeaponChanged;
            _player.MaxAmmoChanged  += _hudPresenter.OnMaxAmmoChanged;
            _player.Died            += OnPlayerDeath;
            _player.HealthChanged   += _hudPresenter.OnHealthChanged;
            _enemySpawner.EnemyDied += _levelSystem.AddExperience;

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
        }

        public override void Dispose()
        {
            _gameOverPresenter.Restarted -= RestartGame;
            _pausePresenter.Restarted    -= RestartGame;
            _pausePresenter.Continued    -= ContinueGame;
            _pausePresenter.MenuReturned -= ReturnToMenu;

            _player.AmmoChanged     -= _hudPresenter.OnAmmoChanged;
            _player.WeaponChanged   -= _hudPresenter.OnWeaponChanged;
            _player.MaxAmmoChanged  -= _hudPresenter.OnMaxAmmoChanged;
            _player.Died            -= OnPlayerDeath;
            _player.HealthChanged   -= _hudPresenter.OnHealthChanged;
            _enemySpawner.EnemyDied -= _levelSystem.AddExperience;

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
            Time.timeScale = 0f;
            _isGamePaused  = true;
            _playerInput.TogglePause(_isGamePaused);
            _pausePresenter.TogglePausePanel(_isGamePaused);
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
            Time.timeScale = 1f;
            _isGamePaused  = false;
            _playerInput.TogglePause(_isGamePaused);
            _pausePresenter.TogglePausePanel(_isGamePaused);
        }

        private void EndGame() => _gameOverPresenter.ShowView();

        private void ReturnToMenu()
        {
            if (_isGamePaused) ContinueGame();
            _saveSystem.SavePlayerProgress(new PlayerData(_levelSystem.GetLevelNumber(), _levelSystem.GetExperience(),
                _player.GetAmmoValue(), _player.GetActiveWeaponIndex(), _player.GetHealthValue()));
            _gameManager.ReturnToMenu();
        }

        private void RestartGame()
        {
            if (_isGamePaused) ContinueGame();
            _saveSystem.ResetPlayerProgress();
            _gameManager.RestartLevel(SceneManager.GetActiveScene().name);
        }

        #endregion
    }
}