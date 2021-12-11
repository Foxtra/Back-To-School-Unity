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

        public async UniTask StartGame(StartParameters parameters) => await LoadGame(parameters);

        public async UniTask ReturnToMenu() => await LoadMenu();

        public async UniTask RestartLevel(string sceneName, EGameModes gameMode) =>
            await StartGame(new StartParameters(true, sceneName, gameMode));

        public async UniTask LoadMenu()
        {
            _currentModel?.Dispose();
            await SceneManager.LoadSceneAsync(EScenes.MainMenu.ToStringCached());
            InitializeScene(EGame.MenuCamera);
            _currentModel = new MainMenuModel(this, _viewFactory);
        }

        public async UniTask LoadGame(StartParameters parameters)
        {
            _currentModel?.Dispose();
            _startParameters = parameters;
            await SceneManager.LoadSceneAsync(parameters.NextScene);
            InitializeScene(EGame.PlayerCamera);

            if (_startParameters == null)
                _startParameters = new StartParameters(true);
            _currentModel = new GameModel(_saveSystem, this, _resourceManager, _inputManager, _viewFactory, _mainCamera, _startParameters);
        }

        private void Initialize()
        {
            _saveSystem            = new SaveSystem();
            _resourceManager       = new ResourceManager();
            _systemResourceManager = new ResourceManager();
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