﻿using System.Collections;
using Assets.BackToSchool.Scripts.Progression;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private SaveSystem _saveSystem;
        public static GameManager Instance { get; private set; }

        public void ExitGame() { Application.Quit(); }

        public void StartGame(GameParameters parameters) => StartCoroutine(LoadGame(parameters));

        public IEnumerator LoadGame(GameParameters parameters)
        {
            AddParametersForLevel(parameters);
            var asyncOp = SceneManager.LoadSceneAsync(parameters.NextScene);
            while (!asyncOp.isDone) { yield return null; }

            var game = FindObjectOfType<Game>();
            game.Initialize(_saveSystem, parameters);
        }

        private void Awake()
        {
            _saveSystem = new SaveSystem();

            if (Instance == null)
                Instance = this;
            else if (Instance == this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void AddParametersForLevel(GameParameters param)
        {
            if (param.IsNewGame)
                param.SetInitialLevel(0);
            else if (_saveSystem.IsSaveDataExists())
            {
                var playerLevel = _saveSystem.GetPlayerLevel();
                param.SetInitialLevel(playerLevel);
            }
        }
    }
}