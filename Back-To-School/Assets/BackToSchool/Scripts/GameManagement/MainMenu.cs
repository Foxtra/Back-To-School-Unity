using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _exitGameButton.onClick.AddListener(ExitGame);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            SceneManager.LoadScene(SceneNames.MainScene);
        }
    }
}