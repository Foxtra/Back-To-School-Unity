using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Progression;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        private ISaveSystem _saveSystem;
        private IResourceManager _resourceManager;
        private StartParameters _startParameters;
        public static GameManager Instance { get; private set; }

        public bool IsSaveDataExists() => _saveSystem != null && _saveSystem.IsSaveDataExists();

        public void ExitGame() { Application.Quit(); }

        public async void StartGame(StartParameters parameters) => await LoadGame(parameters);
        public async void ReturnToMenu()                        => await LoadMenu();
        public       void RestartLevel(string sceneName)        => StartGame(new StartParameters(true, sceneName));

        public void InitializeMenu() => Instantiate(_resourceManager.GetPrefab("MainMenu"));

        public void InitializeMainScene()
        {
            var game = Instantiate(_resourceManager.GetPrefab("Game")).GetComponent<Game>();
            if (_startParameters == null)
                _startParameters = new StartParameters(true);
            game.Initialize(_saveSystem, this, _resourceManager, _startParameters);
        }

        public async UniTask LoadMenu() => await SceneManager.LoadSceneAsync(SceneNames.MainMenu.ToString());

        public async UniTask LoadGame(StartParameters parameters)
        {
            _startParameters = parameters;
            await SceneManager.LoadSceneAsync(parameters.NextScene);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance         = this;
            _saveSystem      = new SaveSystem();
            _resourceManager = new ResourceManager();

            DontDestroyOnLoad(gameObject);
        }
    }
}