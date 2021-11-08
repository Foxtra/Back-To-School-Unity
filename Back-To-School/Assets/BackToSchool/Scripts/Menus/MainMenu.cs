﻿using Assets.BackToSchool.Scripts.GameManagement;
using Assets.BackToSchool.Scripts.UI;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuPresenter _mainMenuPresenter;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager                         =  GameManager.Instance;
            _mainMenuPresenter.ExitTriggered     += ExitGame;
            _mainMenuPresenter.StartTriggered    += StartGame;
            _mainMenuPresenter.ContinueTriggered += ContinueGame;
        }

        private void ContinueGame() => StartCoroutine(_gameManager.StartGame(new GameParameters(false)));

        private void ExitGame() => _gameManager.ExitGame();

        private void StartGame() => StartCoroutine(_gameManager.StartGame(new GameParameters(true)));
    }
}