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

        public void StartGame(StartParameters parameters)              => StartCoroutine(LoadGame(parameters));
        public void ReturnToMenu()                                     => SceneManager.LoadScene(SceneNames.MainMenu.ToString());
        public void RestartLevel(string sceneName, GameModes gameMode) => StartGame(new StartParameters(true, sceneName, gameMode));

        public IEnumerator LoadGame(StartParameters parameters)
        {
            var asyncOp = SceneManager.LoadSceneAsync(parameters.NextScene);
            while (!asyncOp.isDone)
                yield return null;

            var game = FindObjectOfType<Game>();
            game.Initialize(_saveSystem, Instance, parameters);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance    = this;
            _saveSystem = new SaveSystem();

            DontDestroyOnLoad(gameObject);
        }
    }
}