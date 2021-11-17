using System.Collections;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Progression;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private SaveSystem _saveSystem;
        public static GameManager Instance { get; private set; }

        public bool IsSaveDataExists() => _saveSystem != null && _saveSystem.IsSaveDataExists();

        public void ExitGame() { Application.Quit(); }

        public void StartGame(GameParameters parameters) => StartCoroutine(LoadGame(parameters));
        public void ReturnToMenu()                       => SceneManager.LoadScene(SceneNames.MainMenu.ToString());
        public void RestartLevel(string sceneName)       => StartGame(new GameParameters(true, sceneName));

        public IEnumerator LoadGame(GameParameters parameters)
        {
            AddParametersForLevel(parameters);
            var asyncOp = SceneManager.LoadSceneAsync(parameters.NextScene);
            while (!asyncOp.isDone)
                yield return null;

            var game = FindObjectOfType<Game>();
            game.Initialize(_saveSystem, Instance, parameters);
        }

        private void Awake()
        {
            _saveSystem = new SaveSystem();

            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void AddParametersForLevel(GameParameters param)
        {
            if (param.IsNewGame)
                param.SetInitialLevel(0);
            else if (IsSaveDataExists())
            {
                var playerLevel = _saveSystem.GetPlayerLevel();
                param.SetInitialLevel(playerLevel);
            }
        }
    }
}