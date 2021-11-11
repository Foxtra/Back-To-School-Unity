namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class GameParameters
    {
        public GameParameters(bool isNew) : this(isNew, Constants.SceneNames.MainScene.ToString()) { }

        public GameParameters(bool isNew, string scene)
        {
            IsNewGame = isNew;
            NextScene = scene;
        }

        public string NextScene { get; private set; }
        public bool IsNewGame { get; private set; }
        public int InitialLevel { get; private set; }

        public void SetNextScene(Constants.SceneNames scene) => NextScene = scene.ToString();
        public void SetIsNewGame(bool isNew)                 => IsNewGame = isNew;
        public void SetInitialLevel(int level)               => InitialLevel = level;
    }
}