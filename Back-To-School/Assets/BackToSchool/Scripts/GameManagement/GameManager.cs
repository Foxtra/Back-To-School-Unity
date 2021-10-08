using System;
using Assets.BackToSchool.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _PausePanel;
        [SerializeField] private Button _pauseRestartButton;
        [SerializeField] private Button _pauseContinueButton;

        private Button _gameOverRestartButton;
        private GameObject _player;
        private PlayerInteracting _playerInteracting;

        private float _gameOverDelay = 1f;

        private bool _isGamePaused;

        private void Start()
        {
            _gameOverRestartButton = _gameOverPanel.GetComponentInChildren<Button>();
            _gameOverRestartButton.onClick.AddListener(RestartGame);

            _pauseRestartButton.onClick.AddListener(RestartGame);
            _pauseContinueButton.onClick.AddListener(ContinueGame);

            _player = GameObject.FindGameObjectWithTag("Player");
            _playerInteracting = _player.GetComponent<PlayerInteracting>();
            _playerInteracting.OnDeath += GameManager_OnPlayerDeath;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (_isGamePaused)
                {
                    ContinueGame();
                }
                else
                {
                    StopGame();
                }
            }
        }

        private void StopGame()
        {
            Time.timeScale = 0f;
            _PausePanel.SetActive(true);
            _isGamePaused = true;
        }

        private void ContinueGame()
        {
            Time.timeScale = 1f;
            _PausePanel.SetActive(false);
            _isGamePaused = false;
        }

        private void GameManager_OnPlayerDeath(object sender, EventArgs args)
        {
            Invoke(nameof(EndGame), _gameOverDelay);
        }

        private void EndGame()
        {
            _gameOverPanel.SetActive(true);
        }

        private void RestartGame()
        {
            if (_isGamePaused)
            {
                ContinueGame();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}