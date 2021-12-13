using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Models;
using Assets.BackToSchool.Scripts.Parameters;
using Assets.BackToSchool.Scripts.Progression;
using Assets.BackToSchool.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        private IAudioManager _audioManager;
        private IInputManager _inputManager;
        private IResourceManager _resourceManager;
        private ISystemResourceManager _systemResourceManager;
        private ISaveSystem _saveSystem;
        private IViewFactory _viewFactory;
        private IModel _currentModel;
        private Camera _mainCamera;
        private StartParameters _startParameters;
        public static GameManager Instance { get; private set; }

        public bool IsSaveDataExists() => _saveSystem != null && _saveSystem.IsSaveDataExists();

        public void ExitGame() => Application.Quit();

        public async UniTask StartGame(StartParameters parameters) => await LoadScene(parameters);

        public async UniTask ContinueGame()
        {
            var parameters = _saveSystem.LoadLevelParameters();

            await LoadScene(new StartParameters(false, parameters.Scene, parameters.Mode, parameters.LevelNumber));
        }

        public async UniTask ReturnToMenu() => await LoadMenu();

        public async UniTask RestartLevel(LevelParameters parameters)
        {
            if (parameters.LevelNumber == Constants.Level.InitialLevel)
                await StartGame(new StartParameters(true, parameters.Scene, parameters.Mode, parameters.LevelNumber));
            else
                await StartGame(new StartParameters(false, parameters.Scene, parameters.Mode, parameters.LevelNumber, false));
        }

        public bool IsLastLevel(LevelParameters parameters)
        {
            if (parameters.LevelNumber == Constants.Level.MaxLevel)
                return true;

            StartGame(PrepareNextLevelParams(parameters));
            return false;
        }

        public async UniTask LoadMenu()
        {
            _currentModel?.Dispose();
            _audioManager?.Dispose();
            await SceneManager.LoadSceneAsync(EScenes.MainMenu.ToStringCached());
            InitializeScene(EGame.MenuCamera);
            _currentModel = new MainMenuModel(this, _viewFactory);
        }

        public async UniTask LoadScene(StartParameters parameters)
        {
            _currentModel?.Dispose();
            _audioManager?.Dispose();
            _startParameters = parameters;
            await SceneManager.LoadSceneAsync(parameters.Scene.ToStringCached());
            InitializeScene(EGame.PlayerCamera);

            _currentModel = new GameModel(_saveSystem, this, _resourceManager, _inputManager, _viewFactory, _mainCamera, _audioManager,
                _startParameters);
        }

        private StartParameters PrepareNextLevelParams(LevelParameters previousParams)
        {
            var isNew = false;
            var scene = Constants.Level.SceneLevels[previousParams.LevelNumber + 1];
            var mode = previousParams.Mode == EGameModes.KillEnemies ? EGameModes.SurviveTime : EGameModes.KillEnemies;
            var levelNumber = previousParams.LevelNumber + 1;

            return new StartParameters(isNew, scene, mode, levelNumber, false);
        }

        private void Initialize()
        {
            _saveSystem            = new SaveSystem();
            _resourceManager       = new ResourceManager();
            _systemResourceManager = new ResourceManager();
            _audioManager          = new AudioManager(_systemResourceManager);
        }

        private void InitializeScene(EGame cameraType)
        {
            _inputManager = _resourceManager.CreateInputManager();
            _mainCamera   = _resourceManager.CreateCamera(cameraType);
            var uiRoot = _resourceManager.CreateUIRoot(_mainCamera);

            _viewFactory = new ViewFactory(_systemResourceManager, uiRoot);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Initialize();

            DontDestroyOnLoad(gameObject);
        }
    }
}