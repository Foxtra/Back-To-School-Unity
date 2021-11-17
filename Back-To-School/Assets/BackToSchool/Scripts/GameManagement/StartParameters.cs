using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class StartParameters
    {
        public StartParameters(bool isNew) : this(isNew, SceneNames.MainScene.ToString()) { }

        public StartParameters(bool isNew, string scene)
        {
            IsNewGame = isNew;
            NextScene = scene;
        }

        public string NextScene { get; private set; }
        public bool IsNewGame { get; private set; }

        public void SetNextScene(SceneNames scene) => NextScene = scene.ToString();
        public void SetIsNewGame(bool isNew)       => IsNewGame = isNew;
    }
}