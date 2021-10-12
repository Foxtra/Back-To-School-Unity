using Assets.BackToSchool.Scripts.Constants;
using Assets.BackToSchool.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuPresenter _mainMenuPresenter;

        private void Awake()
        {
            _mainMenuPresenter.Exiting  += ExitGame;
            _mainMenuPresenter.Starting += StartGame;
        }

        private void ExitGame() { Application.Quit(); }

        private void StartGame() { SceneManager.LoadScene(SceneNames.MainScene); }
    }
}