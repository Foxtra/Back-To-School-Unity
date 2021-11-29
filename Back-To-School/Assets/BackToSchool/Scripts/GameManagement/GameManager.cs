using System.Collections;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces;
using Assets.BackToSchool.Scripts.Progression;
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

        public void StartGame(StartParameters parameters) => StartCoroutine(LoadGame(parameters));
        public void ReturnToMenu()                        => StartCoroutine(LoadMenu());
        public void RestartLevel(string sceneName)        => StartGame(new StartParameters(true, sceneName));

        public void InitializeMenu() => Instantiate(_resourceManager.GetPrefab("MainMenu"));

        public void InitializeMainScene()
        {
            var game = Instantiate(_resourceManager.GetPrefab("Game")).GetComponent<Game>();
            if (_startParameters == null)
                _startParameters = new StartParameters(true);
            game.Initialize(_saveSystem, this, _resourceManager, _startParameters);
        }

        public IEnumerator LoadMenu()
        {
            var asyncOp = SceneManager.LoadSceneAsync(SceneNames.MainMenu.ToString());
            while (!asyncOp.isDone)
                yield return null;
        }

        public IEnumerator LoadGame(StartParameters parameters)
        {
            _startParameters = parameters;
            var asyncOp = SceneManager.LoadSceneAsync(parameters.NextScene);
            while (!asyncOp.isDone)
                yield return null;
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