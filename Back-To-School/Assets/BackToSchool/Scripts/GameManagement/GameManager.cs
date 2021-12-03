using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Inputs;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Interfaces.UI;
using Assets.BackToSchool.Scripts.Models;
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
        private BaseModel _currentModel;
        private Camera _mainCamera;
        private StartParameters _startParameters;
        public static GameManager Instance { get; private set; }

        public bool IsSaveDataExists() => _saveSystem != null && _saveSystem.IsSaveDataExists();

        public void ExitGame() { Application.Quit(); }

        public async void StartGame(StartParameters parameters) => await LoadGame(parameters);
        public async void ReturnToMenu()                        => await LoadMenu();
        public       void RestartLevel(string sceneName)        => StartGame(new StartParameters(true, sceneName));

        public async UniTask LoadMenu()
        {
            _currentModel?.Dispose();
            await SceneManager.LoadSceneAsync(EScenes.MainMenu.ToStringCached());
            InitializeScene(EScenes.MainMenu);
            _currentModel = new MainMenuModel(this, _viewFactory);
        }

        public async UniTask LoadGame(StartParameters parameters)
        {
            _currentModel?.Dispose();
            _startParameters = parameters;
            await SceneManager.LoadSceneAsync(parameters.NextScene);
            InitializeScene(EScenes.MainScene);

            if (_startParameters == null)
                _startParameters = new StartParameters(true);
            _currentModel = new GameModel(_saveSystem, this, _resourceManager, _inputManager, _viewFactory, _mainCamera, _startParameters);
        }

        private void Initialize()
        {
            _saveSystem            = new SaveSystem();
            _resourceManager       = new ResourceManager();
            _systemResourceManager = new ResourceManager();
            _inputManager          = new InputManager();
        }

        private void InitializeScene(EScenes scene)
        {
            _mainCamera = _resourceManager.CreateCamera(scene == EScenes.MainMenu ? EGame.MenuCamera : EGame.PlayerCamera);
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