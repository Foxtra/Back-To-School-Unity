using UnityEngine;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class MenuStarter : MonoBehaviour
    {
        private void Start()
        {
            var resourceManager = new ResourceManager();
            var gameManager = GameManager.Instance;
            if (gameManager == null)
                gameManager = Instantiate(resourceManager.GetPrefab("GameManager")).GetComponent<GameManager>();

            gameManager.InitializeMenu();
        }
    }
}