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

        private Button _restartButton;
        private GameObject _player;
        private PlayerInteracting _playerInteracting;

        private float _gameOverDelay = 1f;

        private void Start()
        {
            _restartButton = _gameOverPanel.GetComponentInChildren<Button>();
            _restartButton.onClick.AddListener(RestartGame);

            _player = GameObject.FindGameObjectWithTag("Player");
            _playerInteracting = _player.GetComponent<PlayerInteracting>();
            _playerInteracting.OnDeath += GameManager_OnPlayerDeath;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}