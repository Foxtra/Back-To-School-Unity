using Assets.BackToSchool.Scripts.Enums;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class Starter : MonoBehaviour
    {
        private void Start()
        {
            var resourceManager = new ResourceManager();
            var gameManager = GameManager.Instance;
            if (gameManager == null)
                gameManager = resourceManager.CreatePrefabInstance<GameManager, EGame>(EGame.GameManager);

            LoadMenu(gameManager);
        }

        private async void LoadMenu(GameManager manager) => await manager.LoadMenu();
    }
}